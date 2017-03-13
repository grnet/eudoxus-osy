using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace EudoxusOsy.BusinessModel
{
    public static class ObjectParser<T> where T : class, new()
    {
        private static Type InstanceType { get; set; }
        private static string[] PropertyNames { get; set; }

        static ObjectParser()
        {
            InstanceType = typeof(T);
            PropertyNames = InstanceType.GetProperties(BindingFlags.Public | BindingFlags.Instance).Select(x => x.Name).ToArray();
        }

        public static IDictionary<string, object> GetProperties(T instance)
        {
            if (instance == null)
                throw new ArgumentNullException("instance");

            var collection = new Dictionary<string, object>();
            foreach (var item in PropertyNames)
                collection.Add(item, PropertyAccessor<T>.GetValue(instance, item));
            return collection;
        }

        public static T SetProperties(NameValueCollection collection, T instance = null)
        {
            return SetProperties(collection.ToDictionary(), instance);
        }

        public static T SetProperties(IDictionary<string, object> values, T instance = null)
        {
            if (instance == null)
                instance = new T();

            foreach (var item in values)
                PropertyAccessor<T>.SetValue(instance, item.Key, item.Value);

            return instance;
        }
    }

    public static class PropertyAccessor<T>
    {
        private static Type InstanceType { get; set; }
        private static IDictionary<string, Func<T, object>> Getters { get; set; }
        private static IDictionary<string, Action<T, object>> Setters { get; set; }

        static PropertyAccessor()
        {
            InstanceType = typeof(T);
            Getters = new Dictionary<string, Func<T, object>>();
            Setters = new Dictionary<string, Action<T, object>>();
        }

        public static object GetValue(T instance, string propertyName)
        {
            var getter = GetGetter(propertyName);
            return getter(instance);
        }

        public static void SetValue(T instance, string propertyName, object value)
        {
            var setter = GetSetter(propertyName);
            setter(instance, value);
        }

        private static Func<T, object> GetGetter(string propertyName)
        {
            var getter = (Func<T, object>)null;
            if (!Getters.TryGetValue(propertyName, out getter))
            {
                getter = GetGetterLambda(propertyName).Compile();
                Getters[propertyName] = getter;
            }
            return getter;
        }

        private static Expression<Func<T, object>> GetGetterLambda(string propertyName)
        {
            var pi = InstanceType.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            var parameterX = Expression.Parameter(InstanceType, "x");
            var memberX = Expression.MakeMemberAccess(parameterX, pi);
            return Expression.Lambda<Func<T, object>>(Expression.Convert(memberX, typeof(object)), parameterX);
        }

        private static Action<T, object> GetSetter(string propertyName)
        {
            var setter = (Action<T, object>)null;
            if (!Setters.TryGetValue(propertyName, out setter))
            {
                setter = GetSetterLambda(propertyName).Compile();
                Setters[propertyName] = setter;
            }
            return setter;
        }

        private static Expression<Action<T, object>> GetSetterLambda(string propertyName)
        {
            var pi = InstanceType.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            var parameterX = Expression.Parameter(InstanceType, "x");
            var parameterValue = Expression.Parameter(typeof(object), "value");
            var assign = Expression.Assign(Expression.Property(parameterX, pi), Expression.Convert(parameterValue, pi.PropertyType));
            return Expression.Lambda<Action<T, object>>(assign, parameterX, parameterValue);
        }
    }
}