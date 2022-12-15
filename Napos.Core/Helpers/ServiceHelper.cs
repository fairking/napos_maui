using Microsoft.Extensions.DependencyInjection;
using Napos.Core.Abstract;
using Napos.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Napos.Core.Helpers
{
    public static class ServiceHelper
    {
        /// <summary>
        /// Scans for all IService interfaces and registers it into the IServiceCollection.
        /// Eg. the following class "MyService : IScopedService" will be registered as "serviceCollection.AddScoped<MyService>()"
        /// Eg. the following class "MyService : IService<IMyService>" will be registered as "serviceCollection.AddSingleton<IMyService, MyService>()"
        /// IMultiService allows to register multiple services under one interface.
        /// </summary>
        public static void RegisterAllServices(this IServiceCollection serviceCollection, params Assembly[] assemblies)
        {
            var allServices = GetAllServices(assemblies);

            foreach (var service in allServices)
            {
                if (typeof(ITransientService).IsAssignableFrom(service))
                {
                    var inter = service.GetInterfaces().SingleOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ITransientService<>));
                    if (inter != null)
                    {
                        if (!typeof(IMultiService).IsAssignableFrom(service))
                            serviceCollection.RemoveIfExists(inter);

                        serviceCollection.AddTransient(inter.GetGenericArguments()[0], service);
                    }
                    else
                    {
                        if (!typeof(IMultiService).IsAssignableFrom(service))
                            serviceCollection.RemoveIfExists(service);

                        serviceCollection.AddTransient(service);
                    }
                }
                else if (typeof(IScopedService).IsAssignableFrom(service))
                {
                    var inter = service.GetInterfaces().SingleOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IScopedService<>));
                    if (inter != null)
                    {
                        if (!typeof(IMultiService).IsAssignableFrom(service))
                            serviceCollection.RemoveIfExists(inter);

                        serviceCollection.AddScoped(inter.GetGenericArguments()[0], service);
                    }
                    else
                    {
                        if (!typeof(IMultiService).IsAssignableFrom(service))
                            serviceCollection.RemoveIfExists(service);

                        serviceCollection.AddScoped(service);
                    }
                }
                else
                {
                    var inter = service.GetInterfaces().SingleOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IService<>));
                    if (inter != null)
                    {
                        if (!typeof(IMultiService).IsAssignableFrom(service))
                            serviceCollection.RemoveIfExists(inter);

                        serviceCollection.AddSingleton(inter.GetGenericArguments()[0], service);
                    }
                    else
                    {
                        if (!typeof(IMultiService).IsAssignableFrom(service))
                            serviceCollection.RemoveIfExists(service);

                        serviceCollection.AddSingleton(service);
                    }
                }
            }
        }

        public static Type[] GetAllServices(Assembly[] assemblies)
        {
            var allAssemblies = new Assembly[] { typeof(ServiceHelper).Assembly }.Concat(assemblies).ToArray();

            var allServices = ReflectionHelper.GetAllConcreteTypesDerivedFrom<IService>(allAssemblies);

            return allServices;
        }

        public static Type[] GetAllApiServices(Assembly[] assemblies)
        {
            var services = GetAllServices(assemblies);

            return services.Where(x => x.GetCustomAttribute<ApiAttribute>() != null).ToArray();
        }

        public static void RemoveIfExists(this IServiceCollection serviceCollection, Type serviceType)
        {
            var existingServices = serviceCollection.Where(x => x.ServiceType == serviceType).ToList();

            foreach (var service in existingServices)
            {
                serviceCollection.Remove(service);
            }
        }

        /// <summary>
        /// Validates relationships between services.
        /// Throws exception if there are services with dependency loops.
        /// </summary>
        public static void ValidateAllServices(this IServiceCollection serviceCollection)
        {
            // Create a list of dependencies
            var services = new HashSet<Type>(serviceCollection.Select(x => x.ImplementationType ?? x.ServiceType));

            var deptree = new Dictionary<Type, ISet<Type>>(); // Dictionary<TargetService, List<DependentServices>>()

            // Create
            foreach (var service in services)
            {
                var deps = new HashSet<Type>();

                deps.AddRange(
                    service.GetRuntimeFields()
                        .Where(x => !x.IsStatic && services.Any(s => s.Equals(x.FieldType)))
                        .Select(x => x.FieldType)
                );

                deps.AddRange(
                    service.GetRuntimeProperties()
                        .Where(x => services.Any(s => s.Equals(x.PropertyType)))
                        .Select(x => x.PropertyType)
                );

                deptree.Add(service, deps);
            }

            var exceptions = new[] { "DiagnosticListener", "HtmlEncoder", "JavaScriptEncoder", "UrlEncoder", "ArrayPool`1", "DistributedContextPropagator", "App" };

            // Validate loops
            foreach (var dep in deptree)
            {
                if (exceptions.Contains(dep.Key.Name))
                    continue;

                // Validate hierarhy
                var lifetime = serviceCollection.Where(x => dep.Key.Equals(x.ImplementationType ?? x.ServiceType)).Select(x => x.Lifetime).First();
                foreach (var dep2 in dep.Value)
                {
                    var dep_lifetime = serviceCollection.Where(x => dep2.Equals(x.ImplementationType ?? x.ServiceType)).Select(x => x.Lifetime).First();
                    if (lifetime == ServiceLifetime.Singleton && dep_lifetime == ServiceLifetime.Scoped)
                        throw new Exception($"The service '{dep2.Name}' is less accesible than '{dep.Key.Name}'.");
                }

                // Level 1 loop
                if (dep.Value.Contains(dep.Key))
                    throw new Exception($"Dependency loop. The service '{dep.Key.Name}' depends on itself.");

                // Level 2 loop
                foreach (var child in dep.Value)
                {
                    var child_s = serviceCollection.Where(x => x.ImplementationType != null && x.ImplementationType.Equals(child)).ToList();
                    if (child_s.Count > 1)
                        throw new Exception($"The service '{child.Name}' has been registered more than one time.");

                    var child_imp = child_s.SingleOrDefault()?.ServiceType;
                    var child_dep = deptree.Where(x => x.Key.Equals(child) || (child_imp != null && x.Key.Equals(child_imp))).ToList();
                    if (child_dep.Count > 1)
                        throw new Exception($"The service '{child.Name}' has been registered more than one time.");

                    if (child_dep.Single().Value.Contains(dep.Key))
                        throw new Exception($"Dependency loop. The service '{dep.Key.Name}=>{child.Name}' depends on the parent.");

                    // Level 3 loop
                    foreach (var child2 in child_dep.Single().Value)
                    {
                        var child2_s = serviceCollection.Where(x => x.ImplementationType != null && x.ImplementationType.Equals(child2)).ToList();
                        if (child2_s.Count > 1)
                            throw new Exception($"The service '{child2.Name}' has been registered more than one time.");

                        var child2_imp = child2_s.SingleOrDefault()?.ServiceType;
                        var child2_dep = deptree.Where(x => x.Key.Equals(child2) || (child2_imp != null && x.Key.Equals(child2_imp))).ToList();
                        if (child2_dep.Count > 1)
                            throw new Exception($"The service '{child2.Name}' has been registered more than one time.");

                        if (child2_dep.Single().Value.Contains(dep.Key))
                            throw new Exception($"Dependency loop. The service '{dep.Key.Name}=>{child.Name}=>{child2.Name}' depends on the parent.");
                    }
                }
            }
        }
    }
}
