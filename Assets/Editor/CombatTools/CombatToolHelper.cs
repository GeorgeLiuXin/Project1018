using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Galaxy
{
    public class CombatToolHelper
    {
        #region CombatReflection
        public static List<Type> FindClassesWithAttribute(Type attrType)
        {
            var types = new List<Type>();

            foreach (var assem in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assem.GetTypes())
                {
                    if (type.IsDefined(attrType, true))
                        types.Add(type);
                }
            }

            types.Sort((x, y) => x.Name.CompareTo(y.Name));

            return types;
        }

        public static object InvokeMethod(object obj, string method, params object[] args)
        {
            return obj.GetType().GetMethod(method).Invoke(obj, args);
        }

        public static bool HasAttribute<T>(Type info)
        {
            return info.GetCustomAttributes(typeof(T), true).Length > 0;
        }

        public static T GetAttribute<T>(Type info) where T : Attribute
        {
            var attrs = info.GetCustomAttributes(typeof(T), true);
            return attrs.Length > 0 ? (T)attrs[0] : null;
        }

        public static bool HasAttribute<T>(MemberInfo info)
        {
            return info.GetCustomAttributes(typeof(T), true).Length > 0;
        }

        public static T GetAttribute<T>(MemberInfo info) where T : Attribute
        {
            var attrs = info.GetCustomAttributes(typeof(T), true);
            return attrs.Length > 0 ? (T)attrs[0] : null;
        }

        public static T GetPropertyValue<T>(object obj, PropertyInfo info)
        {
            return (T)GetPropertyValue(obj, info);
        }

        public static object GetPropertyValue(object obj, PropertyInfo info)
        {
            return info.GetValue(obj, null);
        }
        #endregion
        
    }

}