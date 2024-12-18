using BasicExtensions.SettingExtensions.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace BasicExtensions.SettingExtensions
{
    public static class KeyValueSettingConverter
    {
        public static Dictionary<string, string> ToKeyValuePairs<T>(this T settings) where T : ISetting => settings.Save<T>(typeof(T));

        public static T ToModel<T>(this Dictionary<string, string> pairs) where T : ISetting
        {
            T instance = (T)Activator.CreateInstance(typeof(T));
            return KeyValueSettingConverter.Load<T>(out instance, pairs, typeof(T));
        }

        private static Dictionary<string, string> Save<T>(
          this T settings,
          Type type = null,
          string pref = "")
          where T : ISetting
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            if ((object)settings != null)
            {
                if (type == (Type)null)
                    type = typeof(T);
                PropertyInfo[] properties = type.GetProperties();
            label_23:
                for (int index1 = 0; index1 < properties.Length; ++index1)
                {
                    PropertyInfo propertyInfo = properties[index1];
                    string lowerInvariant1 = (string.IsNullOrWhiteSpace(pref) ? typeof(ObjectAttribute).Name + "." + type.Name : pref + "." + type.Name).ToLowerInvariant();
                    object[] customAttributes = propertyInfo.GetCustomAttributes(false);
                    if (((IEnumerable<object>)customAttributes).Any<object>((Func<object, bool>)(s => s.GetType().Name == typeof(ListAttribute).Name)))
                    {
                        string lowerInvariant2 = (lowerInvariant1 + "." + typeof(ListAttribute).Name + "." + propertyInfo.Name).ToLowerInvariant();
                        object obj = propertyInfo.GetValue((object)settings, (object[])null);
                        if (obj != null)
                        {
                            IList list = obj as IList;
                            int index2 = 0;
                            while (true)
                            {
                                int num = index2;
                                int? count = list?.Count;
                                int valueOrDefault = count.GetValueOrDefault();
                                if (num < valueOrDefault & count.HasValue)
                                {
                                    foreach (KeyValuePair<string, string> keyValuePair in ((ISetting)list[index2]).Save<ISetting>(list[index2].GetType(), string.Format("{0}.{1}", (object)lowerInvariant2, (object)index2)))
                                        dictionary.Add(keyValuePair.Key, keyValuePair.Value);
                                    ++index2;
                                }
                                else
                                    goto label_23;
                            }
                        }
                    }
                    else if (((IEnumerable<object>)customAttributes).Any<object>((Func<object, bool>)(s => s.GetType().Name == typeof(ObjectAttribute).Name)))
                    {
                        string lowerInvariant3 = (lowerInvariant1 + "." + typeof(ObjectAttribute).Name + "." + propertyInfo.Name).ToLowerInvariant();
                        ISetting settings1 = (ISetting)propertyInfo.GetValue((object)settings);
                        foreach (KeyValuePair<string, string> keyValuePair in settings1.Save<ISetting>(settings1?.GetType(), lowerInvariant3 ?? ""))
                            dictionary.Add(keyValuePair.Key, keyValuePair.Value);
                    }
                    else if (((IEnumerable<object>)customAttributes).Any<object>((Func<object, bool>)(s => s.GetType().Name == typeof(ValueAttribute).Name)))
                    {
                        string lowerInvariant4 = (lowerInvariant1 + "." + typeof(ValueAttribute).Name + "." + propertyInfo.Name).ToLowerInvariant();
                        object obj = propertyInfo.GetValue((object)settings);
                        dictionary.Add(lowerInvariant4, obj?.ToString());
                    }
                }
            }
            return dictionary;
        }

        private static T Load<T>(
          out T instance,
          Dictionary<string, string> pairs,
          Type type = null,
          string pref = "")
          where T : ISetting
        {
            if (type == (Type)null)
                type = typeof(T);
            object obj1 = Activator.CreateInstance(type);
            if (pairs != null && pairs.Count > 0)
            {
                foreach (PropertyInfo property in type.GetProperties())
                {
                    string key = string.IsNullOrWhiteSpace(pref) ? typeof(ObjectAttribute).Name + "." + type.Name : pref + "." + type.Name;
                    key = key.ToLowerInvariant();
                    object[] customAttributes = property.GetCustomAttributes(false);
                    if (((IEnumerable<object>)customAttributes).Any<object>((Func<object, bool>)(s => s.GetType().Name == typeof(ListAttribute).Name)))
                    {
                        key = (key + "." + typeof(ListAttribute).Name + "." + property.Name).ToLowerInvariant();
                        int num = pairs.Where<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>)(s => s.Key.ToLowerInvariant().Contains(key))).Select<KeyValuePair<string, string>, string>((Func<KeyValuePair<string, string>, string>)(s =>
                        {
                            string str = s.Key.ToLowerInvariant().Replace(key.ToLowerInvariant() + ".", "");
                            return str.Substring(0, str.IndexOf('.'));
                        })).Distinct<string>().Count<string>();
                        if (num > 0)
                        {
                            object instance1 = Activator.CreateInstance(property.PropertyType);
                            for (int index = 0; index < num; ++index)
                            {
                                string p = string.Format("{0}.{1}", (object)key, (object)index).ToLowerInvariant();
                                Dictionary<string, string> dictionary = pairs.Where<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>)(s => s.Key.ToLowerInvariant().Contains(p))).ToDictionary<KeyValuePair<string, string>, string, string>((Func<KeyValuePair<string, string>, string>)(c => c.Key.ToLowerInvariant()), (Func<KeyValuePair<string, string>, string>)(c => c.Value));
                                ISetting instance2 = (ISetting)Activator.CreateInstance(property.PropertyType.GetGenericArguments()[0]);
                                KeyValueSettingConverter.Load<ISetting>(out instance2, dictionary, property.PropertyType.GetGenericArguments()[0], p);
                                ((IList)instance1).Add((object)instance2);
                            }
                            property.SetValue(obj1, instance1, (object[])null);
                        }
                    }
                    else if (((IEnumerable<object>)customAttributes).Any<object>((Func<object, bool>)(s => s.GetType().Name == typeof(ObjectAttribute).Name)))
                    {
                        key = (key + "." + typeof(ObjectAttribute).Name + "." + property.Name).ToLowerInvariant();
                        Dictionary<string, string> dictionary = pairs.Where<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>)(s => s.Key.ToLowerInvariant().Contains(key))).ToDictionary<KeyValuePair<string, string>, string, string>((Func<KeyValuePair<string, string>, string>)(c => c.Key.ToLowerInvariant()), (Func<KeyValuePair<string, string>, string>)(c => c.Value));
                        ISetting instance3 = (ISetting)Activator.CreateInstance(property.PropertyType);
                        KeyValueSettingConverter.Load<ISetting>(out instance3, dictionary, property.PropertyType, key);
                        property.SetValue(obj1, (object)instance3, (object[])null);
                    }
                    else if (((IEnumerable<object>)customAttributes).Any<object>((Func<object, bool>)(s => s.GetType().Name == typeof(ValueAttribute).Name)))
                    {
                        key = (key + "." + typeof(ValueAttribute).Name + "." + property.Name).ToLowerInvariant();
                        string text = pairs.Any<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>)(x => x.Key.ToLowerInvariant() == key)) ? pairs.First<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>)(s => s.Key.ToLowerInvariant() == key)).Value : (string)null;
                        if (text != null)
                        {
                            object obj2 = TypeDescriptor.GetConverter(property.PropertyType).ConvertFromInvariantString(text);
                            property.SetValue(obj1, obj2, (object[])null);
                        }
                    }
                }
            }
            else
                obj1 = (object)null;
            instance = (T)obj1;
            return instance;
        }
    }
}
