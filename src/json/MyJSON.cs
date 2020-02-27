using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace json
{
    public static class MyJSON
    {
        public static Dictionary<string, object> Serialize(object obj)
        {
            return SerializeObject(obj);
        }

        private static String GetBackingFieldName(String propertyName) => $"<{propertyName}>k__BackingField";

        private static Dictionary<string, object> SerializeObject(object obj)
        {
            Dictionary<string, object> serialized = new Dictionary<string, object>();

            foreach (FieldInfo fi in obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                Type ft = fi.FieldType;
                object value = fi.GetValue(obj);
                serialized.Add(fi.Name, SerializeValue(ft, value));
            }

            foreach (PropertyInfo fi in obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                var backingFieldName = GetBackingFieldName(fi.Name);
                if(serialized.ContainsKey(backingFieldName))
                {
                    serialized.Remove(backingFieldName);
                }

                Type ft = fi.PropertyType;
                object value = fi.GetValue(obj);
                serialized.Add(fi.Name, SerializeValue(ft, value));
            }

            return serialized;
        }

        private static object SerializeValue(Type vt, object value)
        {
            if(value == null)
            {
                return null;
            }
            else if(vt == typeof(bool) || vt == typeof(char) || vt == typeof(decimal)  || 
                vt == typeof(float) || vt == typeof(double) || vt == typeof(int) ||
                vt == typeof(uint) || vt == typeof(long) || vt == typeof(ulong) || 
                vt == typeof(short) || vt == typeof(ushort) || vt == typeof(string))
            {
                return value;
            }
            else if(vt.IsArray)
            {
                List<object> nested = new List<object>();
                foreach(object item in (Array)value) 
                {
                    nested.Add(SerializeValue(item.GetType(), item));
                }
                return nested.ToArray();
            }
            else if(vt.GetInterfaces().Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
            {
                List<object> nested = new List<object>();
                foreach(object item in (IEnumerable)value) 
                {
                    nested.Add(SerializeValue(item.GetType(), item));
                }
                return nested.ToArray();
            }
            else if(vt.IsClass)
            {
                return SerializeObject(value);
            }

            return null;
        }
    }
}
