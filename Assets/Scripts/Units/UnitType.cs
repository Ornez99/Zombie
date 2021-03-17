using System;
using UnityEngine;

// It is very important that UnitType.X is same as name of unit prefab
// in Resources/Prefabs/Units folder.
public enum UnitType  {
    Zombie,
    Human
}

public static class UnitTypeMethods {
    public static UnitType FromString(string str) {
        foreach (UnitType type in Enum.GetValues(typeof(UnitType))) {
            if (type.ToString() == str)
                return type;
        }

        Debug.LogError($"There is no UnitType for string: {str}");
        return UnitType.Zombie;
    }
}
