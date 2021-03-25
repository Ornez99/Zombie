using System;
using UnityEngine;

public static class EnumMethods<T> {
    public static T FromString(string str) {
        T[] t = (T[])Enum.GetValues(typeof(T));
        for(int i = 0; i < t.Length; i++)
        foreach (T type in Enum.GetValues(typeof(T))) {
            
            if (type.ToString() == str)
                return type;
        }

        Debug.LogError($"There is no for string: {str}");
        return t[0];
    }
}