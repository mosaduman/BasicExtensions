using BasicExtensions.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;


namespace BasicExtensions
{
    public static class DataExtensions
    {
        public static string ToJson<T>(this T entity) => JsonConvert.SerializeObject((object)entity);

        public static T ToModelFromJson<T>(this string entity) => JsonConvert.DeserializeObject<T>(entity);

        public static string ToXml<T>(this T entity)
        {
            using (StringWriter output = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create((TextWriter)output))
                {
                    new System.Xml.Serialization.XmlSerializer(typeof(T)).Serialize(xmlWriter, (object)entity);
                    return output.ToString();
                }
            }
        }

        public static T ToModelFromXml<T>(this string data)
        {
            if (string.IsNullOrWhiteSpace(data))
                return default(T);
            using (StringReader stringReader = new StringReader(data))
                return (T)new System.Xml.Serialization.XmlSerializer(typeof(T)).Deserialize((TextReader)stringReader);
        }

        private static void Map<TSource, TDestination>(this TSource source, TDestination destination)
          where TSource : class, new()
          where TDestination : class, new()
        {
            if ((object)source == null || (object)destination == null)
                return;
            List<PropertyInfo> list1 = ((IEnumerable<PropertyInfo>)source.GetType().GetProperties()).ToList<PropertyInfo>();
            List<PropertyInfo> list2 = ((IEnumerable<PropertyInfo>)destination.GetType().GetProperties()).ToList<PropertyInfo>();
            foreach (PropertyInfo propertyInfo1 in list1)
            {
                PropertyInfo sourceProperty = propertyInfo1;
                PropertyInfo propertyInfo2 = list2.Find((Predicate<PropertyInfo>)(item => item.Name.ToLower() == sourceProperty.Name.ToLower()));
                if (propertyInfo2 != (PropertyInfo)null)
                {
                    try
                    {
                        propertyInfo2.SetValue((object)destination, sourceProperty.GetValue((object)source, (object[])null), (object[])null);
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
        }

        public static TDestination Map<TDestination>(this object source) where TDestination : class, new()
        {
            TDestination destination = new TDestination();
            source.Map<object, TDestination>(destination);
            return destination;
        }

        public static TDestination MapJson<TDestination>(this object source) => source.ToJson<object>().ToModelFromJson<TDestination>();

        public static T ToEnum<T>(this string val) where T : Enum
        {
            try
            {
                return (T)Enum.Parse(typeof(T), val);
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

        public static List<ListItem> ToList<T>() where T : Enum => DataExtensions.EnumToList(typeof(T));

        public static List<ListItem> EnumToList(Type type) => Enum.GetValues(type).Cast<int>().Select<int, ListItem>((Func<int, ListItem>)(c => new ListItem()
        {
            Id = c.ToString(),
            Value = Enum.GetName(type, (object)c)
        })).ToList<ListItem>();
    }
}
