using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Napos.Core.Helpers
{
    public static class ReflectionHelper
    {
        public static bool Is<T>(this object obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            return typeof(T).IsAssignableFrom(obj.GetType());
        }

        public static Type[] GetAllConcreteTypesDerivedFrom<T>(params Assembly[] assemblies)
        {
            var types = new List<Type>();

            foreach (var assembly in assemblies)
            {
                types.AddRange(assembly.GetExportedTypes().Where(x => !x.IsAbstract && !x.IsInterface && typeof(T).IsAssignableFrom(x)));
            }

            return types.ToArray();
        }

        public static object CallGenericMethod(this object obj, string methodName, Type[] methodArgs, Type[] genericTypes, params object[] args)
        {
            var method = obj.GetType().GetMethod(methodName ?? throw new ArgumentNullException(nameof(methodName)), methodArgs ?? new Type[0]);
            var generic = method.MakeGenericMethod(genericTypes ?? throw new ArgumentNullException(nameof(genericTypes)));
            return generic.Invoke(obj, args);
        }

        public static async Task<object> CallGenericMethodAsync(this object obj, string methodName, Type[] methodArgs, Type[] genericTypes, params object[] args)
        {
            var method = obj.GetType().GetMethod(methodName, methodArgs);
            var generic = method.MakeGenericMethod(genericTypes);
            return await (Task<object>)generic.Invoke(obj, args);
        }

        public static Assembly GetAssemblyByName(this string assemblyName)
        {
            if (string.IsNullOrWhiteSpace(assemblyName))
                return null;

            try
            {
                return Assembly.Load(assemblyName);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Eg. typeof(IEnumerable<>).IsAssignableFromGenericType(List<int>) gives you "true".
        /// </summary>
        public static bool IsAssignableFromGenericType(this Type genericType, Type givenType)
        {
            if (givenType == null)
                throw new ArgumentNullException(nameof(givenType));

            if (genericType == null)
                throw new ArgumentNullException(nameof(genericType));

            var interfaceTypes = givenType.GetInterfaces();

            foreach (var it in interfaceTypes)
            {
                if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
                    return true;
            }

            if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
                return true;

            Type baseType = givenType.BaseType;
            if (baseType == null)
                return false;

            return genericType.IsAssignableFromGenericType(baseType);
        }

        /// <summary>
        /// Returns true if the given type is assignable to generic type eg. typeof(List<int>).IsAssignableToGenericType(typeof(IEnumerable<>))
        /// </summary>
        public static bool IsAssignableToGenericType(this Type givenType, Type genericType)
        {
            if (givenType == null)
                throw new ArgumentNullException(nameof(givenType));

            if (genericType == null)
                throw new ArgumentNullException(nameof(genericType));

            var interfaceTypes = givenType.GetInterfaces();

            foreach (var it in interfaceTypes)
            {
                if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
                {
                    return true;
                }

                // System.Collections.IEnumerable - is not generic type
                if (it == genericType)
                {
                    return true;
                }
            }

            if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
                return true;

            var baseType = givenType.BaseType;
            return baseType != null && IsAssignableToGenericType(baseType, genericType);
        }

        /// <summary>
        /// Returns true if [propertyInfo] is a generic type of [ofType]
        /// </summary>
        public static bool IsIEnumerableTypeOf(this Type genericType, Type ofType)
        {
            if (genericType == null)
                throw new ArgumentNullException(nameof(genericType));

            if (ofType == null)
                throw new ArgumentNullException(nameof(ofType));

            return genericType.IsAssignableToGenericType(typeof(IEnumerable))
                && genericType.IsGenericType
                && (ofType.IsAssignableFrom(genericType.GetGenericArguments()[0])
                    || ofType.IsAssignableFrom(genericType.GetGenericArguments()[0].BaseType));
        }

        /// <summary> Eg. typeof(IEnumerable<>).GetAssignableGenericTypes(List<int>) gives you "int". </summary>
        public static Type[] GetAssignableGenericArguments(this Type genericType, Type givenType)
        {
            var interfaceTypes = givenType.GetInterfaces();

            foreach (var it in interfaceTypes)
            {
                if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
                    return it.GetGenericArguments();
            }

            if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
                return givenType.GetGenericArguments();

            Type baseType = givenType.BaseType;
            if (baseType == null)
                return null;

            return genericType.GetAssignableGenericArguments(baseType);
        }

        public static object GetDefaultValue(this Type t)
        {
            if (t.IsValueType && Nullable.GetUnderlyingType(t) == null)
                return Activator.CreateInstance(t);
            else
                return null;
        }

        /// <summary>
        /// Given a lambda expression that calls a method, returns the method info.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public static MethodInfo GetMethodInfo(Expression<Action> expression)
        {
            return GetMethodInfo((LambdaExpression)expression);
        }

        /// <summary>
        /// Given a lambda expression that calls a method, returns the method info.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public static MethodInfo GetMethodInfo<T>(Expression<Action<T>> expression)
        {
            return GetMethodInfo((LambdaExpression)expression);
        }

        /// <summary>
        /// Given a lambda expression that calls a method, returns the method info.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public static MethodInfo GetMethodInfo<T, TResult>(Expression<Func<T, TResult>> expression)
        {
            return GetMethodInfo((LambdaExpression)expression);
        }

        /// <summary>
        /// Given a lambda expression that calls a method, returns the method info.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public static MethodInfo GetMethodInfo(LambdaExpression expression)
        {
            var methodExpression = expression.Body as MethodCallExpression;

            if (methodExpression == null)
            {
                throw new ArgumentException("Invalid Expression. Expression should consist of a Method call only.");
            }

            return methodExpression.Method;
        }

        /// <summary>
        /// Given a lambda expression that calls a method, returns the method parameters.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public static object[] GetMethodParameters(Expression<Action> expression)
        {
            return GetMethodParameters((LambdaExpression)expression);
        }

        /// <summary>
        /// Given a lambda expression that calls a method, returns the method parameters.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public static object[] GetMethodParameters<T>(Expression<Action<T>> expression)
        {
            return GetMethodParameters((LambdaExpression)expression);
        }

        /// <summary>
        /// Given a lambda expression that calls a method, returns the method parameters.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public static object[] GetMethodParameters<T, TResult>(Expression<Func<T, TResult>> expression)
        {
            return GetMethodParameters((LambdaExpression)expression);
        }

        /// <summary>
        /// Given a lambda expression that calls a method, returns the method info.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public static object[] GetMethodParameters(LambdaExpression expression)
        {
            var methodExpression = expression.Body as MethodCallExpression;

            if (methodExpression == null)
            {
                throw new ArgumentException("Invalid Expression. Expression should consist of a Method call only.");
            }

            return methodExpression.Arguments.Select(x => GetArgumentValue(x)).ToArray();
        }

        private static object GetArgumentValue(Expression element)
        {
            if (element is ConstantExpression)
            {
                return (element as ConstantExpression).Value;
            }

            var l = Expression.Lambda(Expression.Convert(element, element.Type));
            return l.Compile().DynamicInvoke();
        }

        public static string GetTypeName(this Type type)
        {
            if (type == null)
                return null;

            if (!type.IsGenericType)
            {
                return type.Name;
            }
            else
            {
                var genericTypeName = type.GetGenericTypeDefinition().Name;
                return genericTypeName.Remove(genericTypeName.IndexOf('`')) + string.Join("", type.GetGenericArguments().Select(x => "_" + x.Name));
            }
        }

        public static string GetPropertyPath<TModel, TProperty>(this Expression<Func<TModel, TProperty>> propertyPath)
        {
            if (propertyPath == null)
                return null;

            return string.Join(".", propertyPath.ToString().Split('.').Skip(1));
        }

        public static T MergeObjects<T>(this T target, T source) where T : class
        {
            return (T)target.MergeObjects2(source);
        }

        public static object MergeObjects2(this object target, object source)
        {
            Type t = target.GetType();

            if (t != source.GetType())
                throw new ArgumentException("Target and Source must be of the same type.");

            var properties = t.GetProperties().Where(prop => prop.CanRead && prop.CanWrite);

            foreach (var prop in properties)
            {
                var value = prop.GetValue(source, null);
                if (value != null)
                    prop.SetValue(target, value, null);
            }

            return target;
        }

        public static void AddValueArray<T>(this SerializationInfo info, string name, T[] values)
        {
            info.AddValue($"{name}.Length", values?.Length);
            if (values == null)
                return;

            for (var i = 0; i < values.Length; i++)
                info.AddValue($"{name}[{i}]", values[i]);
        }

        public static T GetValue<T>(this SerializationInfo info, string name)
        {
            return (T)info.GetValue(name, typeof(T));
        }

        public static T[] GetValueArray<T>(this SerializationInfo info, string name)
        {
            var length = GetValue<int?>(info, $"{name}.Length");
            if (length == null)
                return null;

            var result = new T[length.Value];
            for (var i = 0; i < result.Length; i++)
                result[i] = GetValue<T>(info, $"{name}[{i}]");
            return result;
        }

    }
}
