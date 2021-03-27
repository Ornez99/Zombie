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

    private Color32 emptyColor = new Color32(255, 255, 255, 255);

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
        InstantiateHouses();
        SetGroundTextureAndSize();
    }

    private void CreateGroundTexture() {
        groundTexture2D = new Texture2D(Map.Instance.MapSize, Map.Instance.MapSize);
        for (int y = 0; y < Map.Instance.MapSize; y++) {
            for (int x = 0; x < Map.Instance.MapSize; x++) {
                groundTexture2D.SetPixel(x, y, groundColors[1].OutputColor);
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
        int availableIterations = 1000;

        while (availableIterations > 0 && generatedHouses < housesAmount) {
            int houseId = Random.Range(0, houseFloors.Length);
            int houseSize = houseFloors[houseId].width;

            int nodeX = Random.Range(2, Map.Instance.MapSize - houseSize - 2);
            int nodeZ = Random.Range(2, Map.Instance.MapSize - houseSize - 2);

            if (CanSpawnHouse(new Vector2Int(nodeX, nodeZ), houseSize)) {
                InstantiateHouse(new Vector2Int(nodeX, nodeZ), houseId);
                ChangeNodesToNotSpawnable(new Vector2Int(nodeX, nodeZ), houseSize);
                generatedHouses++;
            }
            else {
                availableIterations--;
            }
        }
    }

    private bool CanSpawnHouse(Vector2Int pos, int size) {
        for (int y = pos.y; y < pos.y + size; y++) {
            for (int x = pos.x; x < pos.x + size; x++) {
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

    private void ChangeNodesToNotSpawnable(Vector2Int pos, int size) {
        for (int y = pos.y; y < pos.y + size; y++) {
            for (int x = pos.x; x < pos.x + size; x++) {
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

                for (int i = 0; i < groundColors.Length; i++) {
                    if (color.Equals(groundColors[i].InputColor)) {
                        groundTexture2D.SetPixel(pos.x + x, pos.y + y, groundColors[i].OutputColor);
                        break;
                    }
                }
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

}
