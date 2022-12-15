using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Writers;
using Napos.Core.Attributes;
using Napos.Core.Helpers;
using Napos.Domain;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Text.Json;

namespace Napos.SchemaGenerator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string path;

            if (args == null || args.Length == 0)
            {
                path = Path.Combine(Directory.GetCurrentDirectory(), "schema.json");
                Console.WriteLine("Output path not provided. Defaulting to the current directory: " + path);
            }
            else
            {
                path = Path.Combine(Directory.GetCurrentDirectory(), args[0]);
                Console.WriteLine("Output path provided: " + path);
            }

            GenerateSchema(path);
        }

        static void GenerateSchema(string schemaFileName)
        {
            var document = new OpenApiDocument
            {
                Info = new OpenApiInfo
                {
                    Version = "1.0.0",
                    Title = "Open API Napos Scheme",
                },
                Paths = new OpenApiPaths(),
            };

            // Register All Services
            var repository = new SchemaRepository();
            var generator = new Swashbuckle.AspNetCore.SwaggerGen.SchemaGenerator(
                new SchemaGeneratorOptions()
                {
                    SchemaFilters = new List<ISchemaFilter>()
                    {
                        new CustomSchemaFilter(),
                    }, 
                }, new JsonSerializerDataContractResolver(new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));

            var services = new ServiceCollection();

            ServiceHelper.RegisterAllServices(services, typeof(Bootstrap).Assembly);

            foreach (var service in services)
            {
                if (service.ServiceType.GetCustomAttribute<ApiAttribute>() != null)
                {
                    var paths = CreateApiModelPaths(service.ServiceType, generator, repository);
                    foreach (var path in paths)
                        document.Paths.Add(path.Key, path.Value);
                }
            }

            document.Components = new OpenApiComponents()
            {
                Schemas = repository.Schemas,
            };

            using (var outputString = new StringWriter())
            {
                var writer = new OpenApiJsonWriter(outputString);
                document.SerializeAsV3(writer);
                File.WriteAllText(schemaFileName, outputString.ToString());
            }
        }

        static OpenApiPaths CreateApiModelPaths(Type serviceType, Swashbuckle.AspNetCore.SwaggerGen.SchemaGenerator schemaGenerator, SchemaRepository schemaRepository)
        {
            var paths = new OpenApiPaths();

            var publicMethods = serviceType.GetMethods().Where(x => x.IsPublic && x.GetCustomAttribute<ApiAttribute>() != null).ToArray();

            foreach (var method in publicMethods)
            {
                var path = new OpenApiPathItem()
                {
                    Description = serviceType.GetCustomAttribute<DescriptionAttribute>()?.Description ?? null,
                    Operations = new Dictionary<OperationType, OpenApiOperation>(),
                };

                var pathName = "/" + serviceType.Name.Replace("Service", "") + "/" + method.Name;

                var parameters = method.GetParameters().Select(x => new OpenApiParameter()
                {
                    Name = x.Name,
                    Schema = schemaGenerator.GenerateSchema(x.ParameterType, schemaRepository, parameterInfo: x),
                }).ToList();

                var returnType = ResolveReturnType(method.ReturnType);

                var operation = new OpenApiOperation()
                {
                    Description = method.GetCustomAttribute<DescriptionAttribute>(true)?.Description ?? null,
                    Deprecated = method.GetCustomAttribute<ObsoleteAttribute>(true) != null,
                    Tags = new List<OpenApiTag>() { new OpenApiTag() { Name = serviceType.Name } },
                    OperationId = method.Name,
                    //Parameters = parameters,
                    RequestBody = parameters.Any() 
                        ? new OpenApiRequestBody() { Content = { { "application/json", new OpenApiMediaType() { Schema = parameters.First().Schema } } } }
                        : null,
                    Responses = new OpenApiResponses()
                    {
                        ["200"] = new OpenApiResponse()
                        {
                            Description = "OK",
                            Content = {
                                ["application/json"] = new OpenApiMediaType()
                                {
                                    Schema = returnType != null ? schemaGenerator.GenerateSchema(returnType, schemaRepository) : null,
                                }
                            },
                        }
                    }
                };

                path.AddOperation(OperationType.Post, operation);

                paths.Add(pathName, path);
            }

            return paths;
        }

        public static Type? ResolveReturnType(Type returnType)
        {
            if (returnType == null || returnType == typeof(void) || returnType == typeof(Task))
                return null;

            if (typeof(Task<>).IsAssignableFromGenericType(returnType))
                return returnType.GetGenericArguments().Single();

            return returnType;
        }
    }
}
