using UnityEngine;

public class MapLoader : MonoBehaviour {

    private readonly Color colorWater = new Color(0, 0, 127f / 255f);
    private readonly Color colorWindow = new Color(0, 0, 255f / 255f);
    private readonly Color colorDoors = new Color(127f / 255f, 0, 0);
    private readonly Color colorItem = new Color(0, 255f / 255f, 0);
    private readonly Color colorSpawner = new Color(0, 127f / 255f, 0);
    private readonly Color colorWall = new Color(0, 0, 0);

    public void LoadDefaultMap() {
        Texture2D mapTexture2D = Resources.Load<Texture2D>("Maps/DefaultMap");
        int width = mapTexture2D.width;
        int height = mapTexture2D.height;
        Color[] colors = mapTexture2D.GetPixels();
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                Spawn(colors[x + y * width], x, y);
            }
        }
    }

    private void Spawn(Color color, int x, int y) {
        if (color == colorWall) {
            BuildingFactory.Instance.SpawnBuilding(Map.Instance.Grid[x,y].CenterPos, Quaternion.identity, BuildingType.Wall);
        } else if (color == colorWindow) {
            BuildingFactory.Instance.SpawnBuilding(Map.Instance.Grid[x,y].CenterPos, Quaternion.identity, BuildingType.Window);
        }
        else if (color == colorItem) {
            BuildingFactory.Instance.SpawnBuilding(Map.Instance.Grid[x, y].CenterPos, Quaternion.identity, BuildingType.Chest);
        }
        else if (color == colorDoors) {
            BuildingFactory.Instance.SpawnBuilding(Map.Instance.Grid[x, y].CenterPos, Quaternion.identity, BuildingType.Doors);
        }
        else if (color == colorSpawner) {
            BuildingFactory.Instance.SpawnBuilding(Map.Instance.Grid[x, y].CenterPos, Quaternion.identity, BuildingType.ZombieSpawner);
        }


    }



}
