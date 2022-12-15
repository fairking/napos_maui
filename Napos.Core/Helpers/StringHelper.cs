using Napos.Core.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace Napos.Core.Helpers
{
    public static class StringHelper
    {
        public static string RemoveEndOf(this string str, string toRemove, bool caseSensitive = false)
        {
            if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(toRemove))
                return str;

            if (str.EndsWith(toRemove, caseSensitive ? StringComparison.InvariantCulture : StringComparison.InvariantCultureIgnoreCase))
                str = str.Substring(0, str.Length - toRemove.Length);

            return str;
        }

        public static T XmlDeserialize<T>(this string toDeserialize, T mergeTo = null) where T : class
        {
            return (T)toDeserialize.XmlDeserialize(typeof(T), mergeTo);
        }

        public static string XmlSerialize<T>(this T toSerialize)
        {
            return toSerialize.XmlSerialize();
        }

        public static object XmlDeserialize(this string toDeserialize, Type objectType, object mergeTo = null)
        {
            object result;

            XmlSerializer xmlSerializer = new XmlSerializer(objectType);
            using (StringReader textReader = new StringReader(toDeserialize))
            {
                result = xmlSerializer.Deserialize(textReader);
            }

            if (mergeTo != null)
                result = mergeTo.MergeObjects(result);

            return result;
        }

        public static string XmlSerialize(this object toSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());
            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, toSerialize);
                return textWriter.ToString();
            }
        }

        public static object JsonDeserialize(this string toDeserialize, Type type)
        {
            return JsonConvert.DeserializeObject(toDeserialize, type);
        }

        public static string JsonSerialize(this object toSerialize)
        {
            return JsonConvert.SerializeObject(toSerialize);
        }

        public static T JsonDeserialize<T>(this string toDeserialize)
        {
            return JsonConvert.DeserializeObject<T>(toDeserialize);
        }

        public static string JsonSerialize<T>(this T toSerialize)
        {
            return JsonConvert.SerializeObject(toSerialize);
        }

        public static void TrimObject(this object obj)
        {
            if (obj == null)
                return;

            var type = obj.GetType();

            if (type.GetCustomAttribute<NoTrimAttribute>() != null)
                return;

            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(ø => ø.CanRead && ø.CanWrite).ToList();
            foreach (var property in properties)
            {
                if (property.GetCustomAttribute<NoTrimAttribute>() != null)
                    continue;

                if (property.PropertyType == typeof(string))
                {
                    var propValue = (string)property.GetValue(obj);
                    if (propValue != null)
                        property.SetValue(obj, propValue.Trim());

                    continue;
                }

                if (property.PropertyType == typeof(IEnumerable))
                {
                    var propList = (IEnumerable)property.GetValue(obj);
                    foreach (var item in propList)
                        item.TrimObject();
                }
            }
        }

        public static bool? ToBoolean(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            return value.Equals("true", StringComparison.InvariantCultureIgnoreCase) || value.Equals("1");
        }

        public static string RemoveNewLines(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            return str.Replace("\n", String.Empty).Replace("\r", String.Empty).Replace("\t", String.Empty);
        }

        public static bool IsNullOrEmpty(this string str, bool whiteSpace = true)
        {
            return whiteSpace ? string.IsNullOrWhiteSpace(str) : string.IsNullOrEmpty(str);
        }
    }
}
