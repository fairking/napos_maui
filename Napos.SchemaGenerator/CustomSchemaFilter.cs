using Microsoft.OpenApi.Models;
using Napos.Core.Validation;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Napos.SchemaGenerator
{
    public class CustomSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type == typeof(string))
            {
                var lengthAttr = context.MemberInfo?.GetCustomAttribute<ValLengthAttribute>();
                if (lengthAttr != null)
                {
                    schema.MaxLength = lengthAttr.Max;
                    if (lengthAttr.Min > 0)
                        schema.MinLength = lengthAttr.Min;
                }
                return;
            }
            else if (context.Type == typeof(short) || context.Type == typeof(int) || context.Type == typeof(long) || context.Type == typeof(decimal) || context.Type == typeof(double) || context.Type == typeof(float))
            {
                var rangeAttr = context.MemberInfo?.GetCustomAttribute<ValRangeAttribute>();
                if (rangeAttr != null)
                {
                    schema.Minimum = rangeAttr.Min == double.MinValue ? decimal.MinValue : (decimal)rangeAttr.Min;
                    schema.Maximum = rangeAttr.Max == double.MaxValue ? decimal.MaxValue : (decimal)rangeAttr.Max;
                }
                return;
            }
            else if (context.Type.IsArray)
            {
                var countAttr = context.MemberInfo?.GetCustomAttribute<ValCountAttribute>();
                if (countAttr != null)
                {
                    schema.MinItems = countAttr.MinCount;
                    schema.MaxItems = countAttr.MaxCount;
                }
                return;
            }
            else if (context.Type.IsValueType || schema.Properties.Count == 0)
            {
                return;
            }

            if (schema.Required == null)
                schema.Required = new HashSet<string>();

            foreach (var key in schema.Properties.Keys)
            {
                var propName = ToPascalCase(key);

                var prop = context.Type.GetProperty(propName);
                if (prop != null)
                {
                    var mandatoryAttr = prop.GetCustomAttribute<ValRequiredAttribute>();
                    if (mandatoryAttr != null)
                    {
                        schema.Required.Add(key);
                    }
                }
            }
        }

        /// <summary>
        /// To convert case as swagger may be using lower camel case
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        private static string ToPascalCase(string inputString)
        {
            // If there are 0 or 1 characters, just return the string.
            if (string.IsNullOrEmpty(inputString)) 
                return inputString;

            if (inputString.Length == 1) 
                return inputString.ToUpper();

            return inputString.Substring(0, 1).ToUpper() + inputString.Substring(1);
        }
    }
}
