using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SimpleServicesDashboard.Infrastructure.Helpers;

public static class ObjectToDictionaryHelper
{
    public static IDictionary<string, object> ToDictionary(this object source)
    {
        return source.ToDictionary<object>();
    }

    public static IDictionary<string, T> ToDictionary<T>(this object? source, Func<object, T>? transformFunc = null)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source), "Unable to convert object to a dictionary. The source object is null.");
        }

        var dictionary = new Dictionary<string, T>();
        foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(source))
        {
            AddPropertyToDictionary<T>(property, source, dictionary, transformFunc);
        }

        return dictionary;
    }

    private static void AddPropertyToDictionary<T>(PropertyDescriptor property, object? source, IDictionary<string, T> dictionary, Func<object, T>? transform)
    {
        object? value = property.GetValue(source);
        T? newValue;
        if (transform != null)
        {
            newValue = transform(value);
        }
        else
        {
            newValue = (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFrom(value);
        }

        if (newValue != null && IsOfType<T>(newValue))
        {
            dictionary.Add(property.Name, newValue);
        }
    }

    private static bool IsOfType<T>(object? value)
    {
        return value is T;
    }
}