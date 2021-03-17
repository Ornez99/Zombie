using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingFactory : MonoBehaviour
{
    public static BuildingFactory Instance;

    private Dictionary<BuildingType, GameObject> buildingPrefabs;

    public void Initialize() {
        if (Instance != null && Instance != this) {
            Debug.LogError("There can be only one instance of this script!");
            Destroy(this);
        }

        Instance = this;
        LoadBuildingsFromResources();
    }

    public Building SpawnBuilding(Vector3 position, Quaternion rotation, BuildingType buildingType) {
        Node node = Map.GetNodeFromPos(position);
        Building building = Instantiate(buildingPrefabs[buildingType], position, rotation).GetComponent<Building>();
        node.Buildable = building.Buildable;
        node.Walkable = building.Walkable;
        node.Viewable = building.Viewable;
        node.Smellable = building.Smellable;

        return building;
    }

    private void LoadBuildingsFromResources() {
        buildingPrefabs = new Dictionary<BuildingType, GameObject>();
        GameObject[] buildings = Resources.LoadAll<GameObject>("Prefabs/Buildings");
        foreach (GameObject building in buildings) {
            BuildingType type = EnumMethods<BuildingType>.FromString(building.name);
            buildingPrefabs.Add(type, building);
        }
    }

}
