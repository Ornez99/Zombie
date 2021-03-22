using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    private const int buildingSize = 16;
    private const int spawnBuildingPercent = 40;
    [SerializeField]
    private List<Sprite> buildingFloor;
    [SerializeField]
    private List<Sprite> buildingObjects;

    private int[,] floorMap;
    private int[,] objectsMap;


    [SerializeField]
    private int mapSize = 64;

    private void Awake() {
        GenerateMap();
    }

    private void GenerateMap() {
        int xIterations = mapSize / buildingSize;
        int yIterations = mapSize / buildingSize;

        for (int y = 0; y < yIterations; y++) {
            for (int x = 0; x < xIterations; x++) {
                int spawnBuildingChance = Random.Range(0, 100);
                if (spawnBuildingChance < spawnBuildingPercent) {
                    int buildingType = Random.Range(0, buildingFloor.Count);
                    SpawnBuilding(buildingType);
                }
            }
        }
    }

    private void SpawnBuilding(int buildingType) {
        GenerateFloor(buildingType);
        GenerateObjects(buildingType);
    }

    private void GenerateFloor(int buildingType) {

    }
    private void GenerateObjects(int buildingType) {

    }

}
