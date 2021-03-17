using System;
using UnityEngine;

// It is very important that UnitType.X is same as name of unit prefab
// in Resources/Prefabs/Units folder.
public enum BuildingType {
    Wall,
    Window,
    Chest,
    Doors,
    ZombieSpawner,
    Barricade
}

public static class BuildingTypeMethods {
    public static BuildingType FromString(string str) {
        foreach (BuildingType type in Enum.GetValues(typeof(BuildingType))) {
            if (type.ToString() == str)
                return type;
        }

        Debug.LogError($"There is no BuildingType for string: {str}");
        return BuildingType.Wall;
    }
}