using Microsoft.Extensions.DependencyInjection;
using Napos.Core.Attributes;
using Napos.Core.Exceptions;
using Napos.Core.Helpers;
using Napos.Data;
using Napos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Napos.Services
{
    public class ServiceExecutor
    {
        private readonly IDictionary<string, Type> _services;

        public ServiceExecutor(Assembly[] assemblies)
        {
            var services = ServiceHelper.GetAllApiServices(assemblies);
            _services = services.ToDictionary(x => x.Name.RemoveEndOf("Service"), x => x, StringComparer.InvariantCultureIgnoreCase);
        }

        public async Task<object> ExecuteService(JsRequest request, IServiceProvider scopedServices, CancellationToken cancellationToken = default)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (scopedServices == null)
                throw new ArgumentNullException(nameof(scopedServices));

            if (string.IsNullOrWhiteSpace(request.url))
                throw new ArgumentNullException(nameof(request.url));

            var parts = request.url.Split(new[] { "/", "\\" }, StringSplitOptions.RemoveEmptyEntries);
            var service = parts.Length > 0 ? parts[0].Trim() : null;
            var operation = parts.Length > 1 ? parts[1].Trim() : null;

            if (string.IsNullOrWhiteSpace(service))
                throw new ArgumentNullException(nameof(service));

            if (string.IsNullOrWhiteSpace(operation))
                throw new ArgumentNullException(nameof(operation));

            // Find a service and method
            if (_services.TryGetValue(service, out var serviceType))
            {
                var method = serviceType.GetMethod(operation);
                var apiAttr = method?.GetCustomAttribute<ApiAttribute>();
                if (method != null && apiAttr != null)
                {
                    // Get the model

                    // TODO: At the moment we only expect on model which comes from the body always.
                    var models = method.GetParameters();
                    if (models.Count() > 1)
                        throw new Exception($"Only one parameter of the service method is supported. Passed {models.Count()} parameters '{method.Name}({string.Join(", ", models.Select(x => x.Name))})'.");

                    object? model = null;
                    if (models.Count() == 1)
                    {
                        if (request.data == null)
                            throw new UserException($"Parameter '{models.Single().Name}' not provided for the service '{serviceType.Name}.{method.Name}'.");

                        var modelType = models.Single().ParameterType;

                        model = StringHelper.JsonDeserialize(request.data.ToString(), modelType);

                        if (model == null)
                            throw new UserException($"Parameter '{models.Single().Name}' not provided for the service '{serviceType.Name}.{method.Name}'.");

                        if (!model.IsModelValid(out var validationResults))
                        {
                            throw new UserException(validationResults.Select(x => string.Join(",", x.MemberNames)).ToArray(), validationResults.Select(x => x.ErrorMessage).ToArray());
                        }
                    }

                    //model = Activator.CreateInstance(modelType);
                    //if (controller.HttpContext.Request.HasJsonContentType())
                    //{
                    //    model = await controller.HttpContext.Request.ReadFromJsonAsync(modelType, cancellationToken);
                    //}
                    //else
                    //{
                    //    throw new Exception($"Only json content type supported. Service '{serviceType.Name}.{method.Name}'.");
                    //}
                    ////if (!await (controller as DefaultController).TryUpdateModelAsync(model, modelType))
                    ////{
                    //if (model != null)
                    //{
                    //    if (!controller.TryValidateModel(model))
                    //    {
                    //        var modelErrors = controller.ModelState
                    //            .Where(x => x.Value.Errors.Any())
                    //            .ToDictionary(x => x.Key, x => string.Join("; ", x.Value.Errors.Select(x => x.ErrorMessage)));

                    //        throw new UserException(modelErrors.Keys.ToArray(), modelErrors.Values.ToArray());
                    //    }
                    //}
                    //else
                    //{
                    //    throw new Exception($"Cannot instantiate a model '{modelType.Name}'.");
                    //}
                    //}

                    // Execute the service method

                    var serviceInstance = scopedServices.GetService(serviceType);
                    if (serviceInstance == null)
                        throw new ServiceNotFoundException(serviceType.Name);

                    var parameters = model != null ? new object[] { model } : new object[0];

                    object result = null;
                    DataContext db = null;
                    if (apiAttr.Transactional)
                    {
                        db = scopedServices.GetService<DataContext>();
                        db.BeginTransaction();
                    }

                    try
                    {
                        if (method.ReturnType == typeof(void))
                        {
                            method.Invoke(serviceInstance, parameters);
                            result = null;
                        }
                        else if (method.ReturnType == typeof(Task))
                        {
                            var task = (Task)method.Invoke(serviceInstance, parameters);
                            await task.ConfigureAwait(false);
                            result = null;
                        }
                        else if (method.ReturnType.IsGenericType && method.ReturnType.BaseType == typeof(Task))
                        {
                            var task = (Task)method.Invoke(serviceInstance, parameters);
                            await task.ConfigureAwait(false);
                            result = await Task.Run(() => ((dynamic)task).Result, cancellationToken);
                        }
                        else
                        {
                            result = method.Invoke(serviceInstance, parameters);
                        }

                        if (apiAttr.Transactional)
                        {
                            db.Commit();
                        }

                        return result;
                    }
                    catch
                    {
                        if (apiAttr.Transactional)
                            db.Rollback();

                        throw;
                    }
                }
                else
                {
                    throw new ServiceNotFoundException(service, operation);
                }
            }
            else
            {
                throw new ServiceNotFoundException(service);
            }
        }
    }
}
