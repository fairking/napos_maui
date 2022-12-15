using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Napos.Core.Attributes;
using Napos.Core.Exceptions;
using Napos.Core.Helpers;
using Napos.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Napos.Web.Services
{
    public class ServiceExecutor
    {
        private readonly IDictionary<string, Type> _services;

        public ServiceExecutor(Assembly[] assemblies)
        {
            var services = ServiceHelper.GetAllApiServices(assemblies);
            _services = services.ToDictionary(x => x.Name.RemoveEndOf("Service"), x => x, StringComparer.InvariantCultureIgnoreCase);
        }

        public async Task<IActionResult> ExecuteService(ControllerBase controller, string service, string operation, CancellationToken cancellationToken = default)
        {
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
                        var modelType = models.Single().ParameterType;

                        model = Activator.CreateInstance(modelType);

                        // Try to update model using in-built function. It will get data from query only.
                        await controller.TryUpdateModelAsync(model, modelType, "");

                        // Update model if it's [FromBody] json request.
                        if (controller.HttpContext.Request.HasJsonContentType())
                        {
                            try
                            {
                                var jsonModel = await controller.HttpContext.Request.ReadFromJsonAsync(
                                    modelType,
                                    new JsonSerializerOptions() { AllowTrailingCommas = true },
                                    cancellationToken);
                                model.MergeObjects(jsonModel);
                            }
                            catch (Exception ex)
                            {
                                throw new UserException(ex.Message);
                            }
                        }

                        // Clear old validations and try to validate the model again
                        {
                            controller.ModelState.Clear();
                            if (!controller.TryValidateModel(model))
                            {
                                var modelErrors = controller.ModelState
                                    .Where(x => x.Value.Errors.Any())
                                    .ToDictionary(x => x.Key, x => string.Join("; ", x.Value.Errors.Select(x => x.ErrorMessage)));

                                throw new UserException(modelErrors.Values.ToArray(), modelErrors.Keys.ToArray());
                            }
                        }
                    }

                    // Execute the service method

                    var serviceInstance = controller.HttpContext.RequestServices.GetService(serviceType);
                    if (serviceInstance == null)
                        throw new ServiceNotFoundException(serviceType.Name);

                    var parameters = model != null ? new object[] { model } : new object[0];

                    IActionResult result = null;
                    DataContext db = null;
                    if (apiAttr.Transactional)
                    {
                        db = controller.HttpContext.RequestServices.GetService<DataContext>();
                        db.BeginTransaction();
                    }

                    try
                    {
                        if (method.ReturnType == typeof(void))
                        {
                            method.Invoke(serviceInstance, parameters);
                            result = controller.NoContent();
                        }
                        else if (method.ReturnType == typeof(Task))
                        {
                            var task = (Task)method.Invoke(serviceInstance, parameters);
                            await task.ConfigureAwait(false);
                            result = controller.NoContent();
                        }
                        else if (method.ReturnType.IsGenericType && method.ReturnType.BaseType == typeof(Task))
                        {
                            var task = (Task)method.Invoke(serviceInstance, parameters);
                            await task.ConfigureAwait(false);
                            result = await Task.Run(() => new ObjectResult(((dynamic)task).Result), cancellationToken);
                        }
                        else
                        {
                            var data = method.Invoke(serviceInstance, parameters);
                            result = new ObjectResult(data);
                        }

                        if (apiAttr.Transactional)
                            db.Commit();

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
