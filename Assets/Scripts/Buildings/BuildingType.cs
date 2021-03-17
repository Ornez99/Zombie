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