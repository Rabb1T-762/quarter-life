using System;
using System.Reflection;

namespace _Project.Tests.TestUtilities
{
    public static class TestHelper
    {
        public static void SetPrivateStaticField<T>(Type type, string fieldName, T value)
        {
            var field = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static);
            if (field == null)
            {
                throw new ArgumentException($"Field '{fieldName}' not found in type '{type}'");
            }

            field.SetValue(null, value);
        }

        public static void SetPrivateField(object obj, string fieldName, object value)
        {
            var type = obj.GetType();
            FieldInfo field = null;
            while (type != null)
            {
                field = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
                if (field != null)
                {
                    break;
                }

                type = type.BaseType;
            }

            if (field != null)
            {
                field.SetValue(obj, value);
            }
            else
            {
                throw new InvalidOperationException(
                    $"No field named {fieldName} found in {obj.GetType()} or its base classes.");
            }
        }

        public static void SetProperty(object obj, string propertyName, object value)
        {
            var property = obj.GetType().GetProperty(propertyName,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            property?.SetValue(obj, value);
        }

        public static MethodBase GetPrivateMethod(string methodName, object obj)
        {
            var method = obj.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            
            // throw an exception if method does not exist on the object
            if (method == null)
            {
                throw new MissingMethodException($"Method '{methodName}' not found in object '{obj.GetType()}'");
            }
            
            // return the method found on the object
            return method;
        }
    }
}