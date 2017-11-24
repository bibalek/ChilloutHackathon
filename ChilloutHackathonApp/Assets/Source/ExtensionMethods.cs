using System;
using System.Text;
using UnityEngine;

public static class ExtensionMethods
{
    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
    {
        T component = gameObject.GetComponent<T>() ?? gameObject.gameObject.AddComponent<T>();
        return component;
    }

    public static void Clear(this StringBuilder value)
    {
        value.Length = 0;
        value.Capacity = 0;
    }

    public static float WithPrecision(this float v, int precision)
    {
        float d = Mathf.Pow(10, precision);
        return (float)(Math.Truncate(v * d) / d);
    }
}