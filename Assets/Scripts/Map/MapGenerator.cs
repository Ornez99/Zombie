using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {
    
    [SerializeField]
    private int borderSize = 2;
    [SerializeField]
    private int housesAmount = 5;
    [SerializeField]
    private GameObject ground = null;

    private Color32 emptyColor = new Color32(0, 0, 0, 0);

    private ColorToColor[] groundColors;
    private ColorToBuildingType[] buildingTypeColors;
    private ColorToVector3[] directionColor;

    private Texture2D[] houseFloors;
    private Texture2D[] houseObjects;
    private Texture2D[] houseDirections;

    private bool[,] spawnable;
    private Texture2D groundTexture2D;

    public void GenerateMap() {
        LoadDataFromResources();
        CreateGroundTexture();
        SetBorderSize();
        SetSpawnable();
        CreateWallsAtMapBorder();

        InstantiateHouse(new Vector2Int(Map.Instance.MapSize / 2 - 10, Map.Instance.MapSize / 2 - 10), 0);
        ChangeNodesToNotSpawnable(new Vector2Int(Map.Instance.MapSize / 2 - 10, Map.Instance.MapSize / 2 - 10), houseFloors[0].width, houseFloors[0].height);

        InstantiateHouses();
        SetGroundTextureAndSize();
        PlantTrees();
    }

    private void CreateGroundTexture() {
        groundTexture2D = new Texture2D(Map.Instance.MapSize, Map.Instance.MapSize);
        float[,] grassNoise = Noise.GenerateNoiseMap(Map.Instance.MapSize, 0, 8, 4, 1, 0.5f, Vector2.zero);
        float[,] concreteNoise = Noise.GenerateNoiseMap(Map.Instance.MapSize, 1, 8, 4, 1, 0.5f, Vector2.zero);

        for (int y = 0; y < Map.Instance.MapSize; y++) {
            for (int x = 0; x < Map.Instance.MapSize; x++) {
                float grassStrength = (1f * grassNoise[x, y]) / 2f + 0.5f;
                float concreteStrength = (1f * concreteNoise[x, y]) / 2f;
                groundTexture2D.SetPixel(x, y, new Color(grassStrength, concreteStrength, 0, 0));
            }
        }
    }

    private void SetBorderSize() {
        borderSize = 2;
    }

    private void SetSpawnable() {
        spawnable = new bool[Map.Instance.MapSize, Map.Instance.MapSize];
        for (int y = 0; y < Map.Instance.MapSize; y++) {
            for (int x = 0; x < Map.Instance.MapSize; x++) {
                if (x < 0 + borderSize || y < 0 + borderSize || x > Map.Instance.MapSize - 1 - borderSize || y > Map.Instance.MapSize - 1 - borderSize)
                    spawnable[x, y] = false;
                else
                    spawnable[x, y] = true;
            }
        }
    }

    private void CreateWallsAtMapBorder() {
        for (int y = 0; y < Map.Instance.MapSize; y++) {
            for (int x = 0; x < Map.Instance.MapSize; x++) {
                if (x == 0 || y == 0 || x == Map.Instance.MapSize - 1 || y == Map.Instance.MapSize - 1)
                    BuildingFactory.Instance.SpawnBuilding(new Vector3(x + 0.5f, 0, y + 0.5f), Quaternion.Euler(0, 0, 0), BuildingType.Wall);
            }
        }
    }

    private void LoadDataFromResources() {
        groundColors = Resources.LoadAll<ColorToColor>("ScriptableObjects/ColorToColor");
        buildingTypeColors = Resources.LoadAll<ColorToBuildingType>("ScriptableObjects/ColorToBuildingType");
        directionColor = Resources.LoadAll<ColorToVector3>("ScriptableObjects/ColorToVector3");
        houseFloors = Resources.LoadAll<Texture2D>("Houses/Floors");
        houseObjects = Resources.LoadAll<Texture2D>("Houses/Objects");
        houseDirections = Resources.LoadAll<Texture2D>("Houses/Directions");
    }

    private void InstantiateHouses() {
        int generatedHouses = 0;
        int availableIterations = 50;

        while (availableIterations > 0 && generatedHouses < housesAmount) {
            int houseId = Random.Range(1, houseFloors.Length);
            int houseSizeX = houseFloors[houseId].width;
            int houseSizeY = houseFloors[houseId].height;

            int nodeX = Random.Range(2, Map.Instance.MapSize - houseSizeX - 2);
            int nodeZ = Random.Range(2, Map.Instance.MapSize - houseSizeY - 2);

            if (CanSpawnHouse(new Vector2Int(nodeX, nodeZ), houseSizeX, houseSizeY)) {
                InstantiateHouse(new Vector2Int(nodeX, nodeZ), houseId);
                ChangeNodesToNotSpawnable(new Vector2Int(nodeX, nodeZ), houseSizeX, houseSizeY);
                generatedHouses++;
            }
            else {
                availableIterations--;
            }
        }
    }

    private bool CanSpawnHouse(Vector2Int pos, int sizeX, int sizeY) {
        for (int y = pos.y; y < pos.y + sizeY; y++) {
            for (int x = pos.x; x < pos.x + sizeX; x++) {
                if (spawnable[x, y] == false)
                    return false;
            }
        }

        return true;
    }

    private void InstantiateHouse(Vector2Int pos, int houseId) {
        ChangeFloorUnderHouse(pos, houseId);
        SpawnHouseObjects(pos, houseId);
    }

    private void ChangeNodesToNotSpawnable(Vector2Int pos, int sizeX, int sizeY) {
        for (int y = pos.y; y < pos.y + sizeY; y++) {
            for (int x = pos.x; x < pos.x + sizeX; x++) {
                spawnable[x, y] = false;
            }
        }
    }

    private void ChangeFloorUnderHouse(Vector2Int pos, int houseId) {
        for (int y = 0; y < houseFloors[houseId].height; y++) {
            for (int x = 0; x < houseFloors[houseId].width; x++) {
                Color32 color = houseFloors[houseId].GetPixel(x, y);
                if (color.Equals(emptyColor))
                    continue;

                groundTexture2D.SetPixel(pos.x + x, pos.y + y, color);
                
            }
        }
    }

    private void SpawnHouseObjects(Vector2Int pos, int houseId) {
        for (int y = 0; y < houseObjects[houseId].height; y++) {
            for (int x = 0; x < houseObjects[houseId].width; x++) {
                Color32 color = houseObjects[houseId].GetPixel(x, y);
                if (color.Equals(emptyColor))
                    continue;

                for (int i = 0; i < buildingTypeColors.Length; i++) {
                    if (color.Equals(buildingTypeColors[i].Color)) {
                        GameObject ins = BuildingFactory.Instance.SpawnBuilding(new Vector3(pos.x + x + 0.5f, 0, pos.y + y + 0.5f), Quaternion.Euler(0, 0, 0), buildingTypeColors[i].BuildingType).gameObject;
                        RotateObject(ins, houseId, new Vector2Int(x,y));
                        break;
                    }
                }
            }
        }
    }

    private void RotateObject(GameObject obj, int houseId, Vector2Int pos) {
        Color32 color = houseDirections[houseId].GetPixel(pos.x, pos.y);
        for (int i = 0; i < directionColor.Length; i++) {
            if (color.Equals(directionColor[i].Color)) {
                obj.transform.rotation = Quaternion.Euler(directionColor[i].Vector3);
                return;
            }
        }
    }

    private void SetGroundTextureAndSize() {
        ground.transform.localScale = new Vector3(-0.1f * Map.Instance.MapSize, 1, -0.1f * Map.Instance.MapSize);
        ground.transform.position = new Vector3(Map.Instance.MapSize / 2f, 0, Map.Instance.MapSize / 2f);
        groundTexture2D.Apply();
        ground.GetComponent<Renderer>().material.SetTexture("_Control", groundTexture2D);
    }

    private void PlantTrees() {
        float[,] treesNoise = Noise.GenerateNoiseMap(Map.Instance.MapSize, 2, 8, 4, 1, 0.5f, Vector2.zero);
        int maxIterations = 256;
        int currentIterations = 0;
        while (currentIterations < maxIterations) {
            int gridX = Random.Range(1, Map.Instance.MapSize - 1);
            int gridZ = Random.Range(1, Map.Instance.MapSize - 1);
            Node randomNode = Map.Instance.Grid[gridX, gridZ];
            if (spawnable[gridX,gridZ]) {
                BuildingType type = BuildingType.Tree1;
                if (treesNoise[gridX, gridZ] > 0.66f)
                    type = BuildingType.Tree2;
                else if (treesNoise[gridX, gridZ] > 0.33f)
                    type = BuildingType.Tree3;

                BuildingFactory.Instance.SpawnBuilding(randomNode.CenterPos, Quaternion.Euler(0, Random.Range(0f, 360f), 0), type);
                spawnable[gridX, gridZ] = false;
            }

            currentIterations++;
        }


    }


}
