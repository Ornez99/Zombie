using System;
using UnityEngine;

// It is very important that WeaponType.X is same as name of weapon prefab
// in Resources/Prefabs/Weapons folder.
public enum WeaponType {
    Pistol,
    Hands
}

public static class WeaponTypeMethods {
    public static WeaponType FromString(string str) {
        foreach (WeaponType type in Enum.GetValues(typeof(WeaponType))) {
            if (type.ToString() == str)
                return type;
        }

        Debug.LogError($"There is no WeaponType for string: {str}");
        return WeaponType.Pistol;
    }
}