using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HouseEditor : MonoBehaviour {

    public enum PaintType {
        Ground,
        Buildings
    }

    [SerializeField]
    private Map map = null;
    [SerializeField]
    private BuildingFactory buildingFactory = null;

    [SerializeField]
    private int houseSizeX = 16;
    [SerializeField]
    private int houseSizeZ = 16;

    [SerializeField]
    private PaintType paintType = default;

    [SerializeField]
    private Button buttonWood = null;
    [SerializeField]
    private Button buttonConcrete = null;
    [SerializeField]
    private Button buttonTiles = null;
    [SerializeField]
    private Button buttonGrass = null;

    private Color32 currentColor = new Color32(0xFF, 0x00, 0x00, 0x00);
    [SerializeField]
    [Range(0f, 1f)]
    private float colorStrength = 1f;
    private bool colorOverride = true;

    private Dictionary<BuildingType, GameObject> buildingPrefabs = null;
    private Dictionary<Color32, BuildingType> buildingColors = null;
    private Dictionary<BuildingType, Color32> colorsBuilding = null;
    private Dictionary<Color32, Color32> groundColors = null;
    private Dictionary<Vector3, Color32> buildingRotations = null;

    private Texture2D buildingsTexture2D = null;
    private Texture2D rotationTexture2D = null;
    private Texture2D groundTexture2D = null;
    [SerializeField]
    private GameObject ground = null;
    private int groundLayer = 1 << 8;

    [SerializeField]
    private Transform mouseTransform = null;

    [SerializeField]
    private Slider colorStrengthSlider = null;
    [SerializeField]
    private Toggle colorOverrideToggle = null;
    [SerializeField]
    private Toggle showBuildingsToggle = null;
    [SerializeField]
    private Dropdown buildingTypeDropdown = null;
    [SerializeField]
    private Dropdown rotationTypeDropdown = null;

    [SerializeField]
    private BuildingType currentBuildingType = default;
    [SerializeField]
    private Transform buildingsParent = null;

    [SerializeField]
    private string fileName;

    private void Awake() {
        map.MapSize = Mathf.Max(houseSizeX, houseSizeZ);
        map.Initialize();
        buildingFactory.Initialize();
        
        GameObject.Find("Resolution").transform.localScale = new Vector3(Screen.width / 1920.0f, Screen.height / 1080.0f, 1);

        buildingPrefabs = new Dictionary<BuildingType, GameObject>();
        buildingColors = new Dictionary<Color32, BuildingType>();
        colorsBuilding = new Dictionary<BuildingType, Color32>();
        groundColors = new Dictionary<Color32, Color32>();
        buildingRotations = new Dictionary<Vector3, Color32>();

        GameObject[] buildings = Resources.LoadAll<GameObject>("Prefabs/Buildings");
        ColorToBuildingType[] colorToBuildingTypes = Resources.LoadAll<ColorToBuildingType>("ScriptableObjects/ColorToBuildingType");
        ColorToColor[] colorToColors = Resources.LoadAll<ColorToColor>("ScriptableObjects/ColorToColor");
        ColorToVector3[] colorToVector3s = Resources.LoadAll<ColorToVector3>("ScriptableObjects/ColorToVector3");

        foreach (GameObject building in buildings) {
            BuildingType type = EnumMethods<BuildingType>.FromString(building.name);
            buildingPrefabs.Add(type, building);
        }
        foreach (ColorToBuildingType colorToType in colorToBuildingTypes) {
            buildingColors.Add(colorToType.Color, colorToType.BuildingType);
            colorsBuilding.Add(colorToType.BuildingType, colorToType.Color);
        }
        foreach (ColorToColor colorToColor in colorToColors) {
            groundColors.Add(colorToColor.InputColor, colorToColor.OutputColor);
        }
        foreach (ColorToVector3 colorToVector3 in colorToVector3s) {
            buildingRotations.Add(colorToVector3.Vector3, colorToVector3.Color);
        }

        SetGroundButtons();
        GenerateNewTexture2D();
        LoadBuildingButtons();

        colorOverride = colorOverrideToggle.isOn;
        buildingsParent.gameObject.SetActive(showBuildingsToggle.isOn);
        colorStrength = colorStrengthSlider.value;
    }

    private void SetGroundButtons() {
        buttonWood.onClick.AddListener(() => SetColor(new Color32(0, 0, 255, 0)));
        buttonConcrete.onClick.AddListener(() => SetColor(new Color32(0, 255, 0, 0)));
        buttonTiles.onClick.AddListener(() => SetColor(new Color32(0, 0, 0, 255)));
        buttonGrass.onClick.AddListener(() => SetColor(new Color32(255, 0, 0, 0)));
    }

    private void Update() {
        Vector3 mousePos = Input.mousePosition;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        int posX = 0;
        int posZ = 0;
        if (Physics.Raycast(ray, out hit, 100, groundLayer)) {
            posX = Mathf.FloorToInt(hit.point.x);
            posZ = Mathf.FloorToInt(hit.point.z);
            mouseTransform.position = new Vector3(posX, 0, posZ);
        }

        if (Input.GetMouseButton(1) && !EventSystem.current.IsPointerOverGameObject()) {
            if (paintType == PaintType.Ground) {
                mousePos = Input.mousePosition;
                hit = new RaycastHit();
                ray = Camera.main.ScreenPointToRay(mousePos);
                if (Physics.Raycast(ray, out hit, 100, groundLayer)) {
                    groundTexture2D.SetPixel(posX, posZ, new Color32(0, 0, 0, 0));
                }
            }

            groundTexture2D.Apply();
            ground.GetComponent<Renderer>().material.SetTexture("_Control", groundTexture2D);
        }

        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject()) {
            if (paintType == PaintType.Ground) {
                mousePos = Input.mousePosition;
                hit = new RaycastHit();
                ray = Camera.main.ScreenPointToRay(mousePos);
                if (Physics.Raycast(ray, out hit, 100, groundLayer)) {
                    if (colorOverride) {
                        Color32 previousColor = new Color32(0x00, 0x00, 0x00, 0x00);
                        if (currentColor.r > 0)
                            previousColor.r = (byte)(0xFF * colorStrength);
                        if (currentColor.g > 0)
                            previousColor.g = (byte)(0xFF * colorStrength);
                        if (currentColor.b > 0)
                            previousColor.b = (byte)(0xFF * colorStrength);
                        if (currentColor.a > 0)
                            previousColor.a = (byte)(0xFF * colorStrength);

                        groundTexture2D.SetPixel(posX, posZ, previousColor);
                    }
                    else {
                        Color32 previousColor = groundTexture2D.GetPixel(posX, posZ);
                        if (currentColor.r > 0)
                            previousColor.r = (byte)(0xFF * colorStrength);
                        if (currentColor.g > 0)
                            previousColor.g = (byte)(0xFF * colorStrength);
                        if (currentColor.b > 0)
                            previousColor.b = (byte)(0xFF * colorStrength);
                        if (currentColor.a > 0)
                            previousColor.a = (byte)(0xFF * colorStrength);

                        groundTexture2D.SetPixel(posX, posZ, previousColor);
                    }
                }
                groundTexture2D.Apply();
                ground.GetComponent<Renderer>().material.SetTexture("_Control", groundTexture2D);
            }
            if (paintType == PaintType.Buildings) {
                mousePos = Input.mousePosition;
                hit = new RaycastHit();
                ray = Camera.main.ScreenPointToRay(mousePos);
                if (Physics.Raycast(ray, out hit, 100, groundLayer)) {
                    Node node = Map.GetNodeFromPos(hit.point);

                    if (node.Buildable) {
                        Building bui = BuildingFactory.Instance.SpawnBuilding(node.CenterPos, Quaternion.Euler(0, rotationTypeDropdown.value * 90, 0), currentBuildingType);
                        bui.transform.parent = buildingsParent;
                        node.Buildable = false;
                        buildingsTexture2D.SetPixel(node.XId, node.YId, colorsBuilding[currentBuildingType]);
                        rotationTexture2D.SetPixel(node.XId, node.YId, buildingRotations[new Vector3(0, rotationTypeDropdown.value * 90, 0)]);
                    }
                }
            }
        }

        if (Input.GetMouseButton(1) && !EventSystem.current.IsPointerOverGameObject()) {
            mousePos = Input.mousePosition;
            hit = new RaycastHit();
            ray = Camera.main.ScreenPointToRay(mousePos);
            if (Physics.Raycast(ray, out hit, 100, groundLayer)) {
                Node node = Map.GetNodeFromPos(hit.point);

                if (node.Buildable == false) {
                    node.Buildable = true;
                    buildingsTexture2D.SetPixel(node.XId, node.YId, new Color32(0xFF, 0xFF, 0xFF, 0xFF));
                    if (node.Building != null)
                        Destroy(node.Building.gameObject);
                }
            }
        }
    }

    private void GenerateNewTexture2D() {
        int width = houseSizeX;
        int height = houseSizeZ;

        groundTexture2D = new Texture2D(width, height);
        groundTexture2D.wrapMode = TextureWrapMode.Clamp;
        groundTexture2D.filterMode = FilterMode.Bilinear;

        rotationTexture2D = new Texture2D(width, height);
        rotationTexture2D.wrapMode = TextureWrapMode.Clamp;
        rotationTexture2D.filterMode = FilterMode.Point;

        buildingsTexture2D = new Texture2D(width, height);
        buildingsTexture2D.wrapMode = TextureWrapMode.Clamp;
        buildingsTexture2D.filterMode = FilterMode.Point;

        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                groundTexture2D.SetPixel(x, y, new Color32(0, 0, 0, 0));
                rotationTexture2D.SetPixel(x, y, new Color32(0xFF, 0xFF, 0xFF, 0xFF));
                buildingsTexture2D.SetPixel(x, y, new Color32(0xFF, 0xFF, 0xFF, 0xFF));
            }
        }
        groundTexture2D.Apply();
        rotationTexture2D.Apply();
        buildingsTexture2D.Apply();
        ground.GetComponent<Renderer>().material.SetTexture("_Control", groundTexture2D);
        ground.GetComponent<Renderer>().material.SetTextureScale("_Splat0", new Vector2(width / 4f, height / 4f));
        ground.GetComponent<Renderer>().material.SetTextureScale("_Splat1", new Vector2(width / 4f, height / 4f));
        ground.GetComponent<Renderer>().material.SetTextureScale("_Splat2", new Vector2(width / 4f, height / 4f));
        ground.GetComponent<Renderer>().material.SetTextureScale("_Splat3", new Vector2(width / 4f, height / 4f));
        SetGroundSize(width, height);
    }

    private void SetGroundSize(int width, int height) {
        ground.transform.localScale = new Vector3(-0.1f * width, 1, -0.1f * height);
        ground.transform.position = new Vector3(width / 2f, 0, height / 2f);
    }

    private void LoadBuildingButtons() {
        List<string> dropdownOptions = new List<string>();
        foreach (BuildingType buildingType in buildingColors.Values) {
            dropdownOptions.Add(buildingType.ToString());
        }
        buildingTypeDropdown.AddOptions(dropdownOptions);
    }

    public void SetColor(Color32 color) {
        currentColor = color;
        paintType = PaintType.Ground;
    }

    public void ChangeFileName(string value) {
        fileName = value;
    }

    public void ChangeHouseSizeX(string newSizeX) {
        int sizeX = 0;
        int.TryParse(newSizeX, out sizeX);

        if (sizeX > 0 && sizeX < 100) {
            houseSizeX = sizeX;
            GenerateNewTexture2D();
            map.MapSize = Mathf.Max(houseSizeX, houseSizeZ);
            map.Initialize();
        }
    }

    public void ChangeHouseSizeZ(string newSizeZ) {
        int sizeZ = 0;
        int.TryParse(newSizeZ, out sizeZ);

        if (sizeZ > 0 && sizeZ < 100) {
            houseSizeZ = sizeZ;
            GenerateNewTexture2D();
            map.MapSize = Mathf.Max(houseSizeX, houseSizeZ);
            map.Initialize();
        }
    }

    public void ChangeColorOverride() {
        colorOverride = colorOverrideToggle.isOn;
    }

    public void ChangeColorStrength() {
        colorStrength = colorStrengthSlider.value;
    }

    public void ChangeBuildingType() {
        int dropdownIndex = buildingTypeDropdown.value;
        currentBuildingType = EnumMethods<BuildingType>.FromString(buildingTypeDropdown.options[dropdownIndex].text);
        paintType = PaintType.Buildings;
    }

    public void ShowBuildings() {
        buildingsParent.gameObject.SetActive(showBuildingsToggle.isOn);
    }

    public void SaveTextures() {
        buildingsTexture2D.Apply();
        rotationTexture2D.Apply();

        byte[] bytes = groundTexture2D.EncodeToPNG();
        string floorDirPath = Application.dataPath + "/Resources/Houses/Floors/";
        if (Directory.Exists(floorDirPath)) {
            string path = floorDirPath + "Floor_" + fileName + ".png";
            File.WriteAllBytes(path, bytes);
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
            Texture2D texture = Resources.Load<Texture2D>("Houses/Floors/Floor_" + fileName);
            string assetPath = AssetDatabase.GetAssetPath(texture);
            var importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            TextureImporterSettings texSettings = new TextureImporterSettings();
            importer.ReadTextureSettings(texSettings);
            texSettings.readable = true;
            texSettings.wrapMode = TextureWrapMode.Clamp;
            texSettings.filterMode = FilterMode.Bilinear;
            importer.SetTextureSettings(texSettings);
            UnityEditor.AssetDatabase.Refresh();
#endif
        }

        bytes = buildingsTexture2D.EncodeToPNG();
        string buildingsDirPath = Application.dataPath + "/Resources/Houses/Objects/";
        if (Directory.Exists(buildingsDirPath)) {
            string path = buildingsDirPath + "Objects_" + fileName + ".png";
            File.WriteAllBytes(path, bytes);
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
            Texture2D texture = Resources.Load<Texture2D>("Houses/Objects/Objects_" + fileName);
            string assetPath = AssetDatabase.GetAssetPath(texture);
            var importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            TextureImporterSettings texSettings = new TextureImporterSettings();
            importer.ReadTextureSettings(texSettings);
            texSettings.readable = true;
            texSettings.wrapMode = TextureWrapMode.Clamp;
            texSettings.filterMode = FilterMode.Point;
            importer.SetTextureSettings(texSettings);
            UnityEditor.AssetDatabase.Refresh();
#endif
        }

        bytes = rotationTexture2D.EncodeToPNG();
        string rotationDirPath = Application.dataPath + "/Resources/Houses/Directions/";
        if (Directory.Exists(rotationDirPath)) {
            string path = rotationDirPath + "Directions_" + fileName + ".png";
            File.WriteAllBytes(path, bytes);
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
            Texture2D texture = Resources.Load<Texture2D>("Houses/Directions/Directions_" + fileName);
            string assetPath = AssetDatabase.GetAssetPath(texture);
            var importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            TextureImporterSettings texSettings = new TextureImporterSettings();
            importer.ReadTextureSettings(texSettings);
            texSettings.readable = true;
            texSettings.wrapMode = TextureWrapMode.Clamp;
            texSettings.filterMode = FilterMode.Point;
            importer.SetTextureSettings(texSettings);
            UnityEditor.AssetDatabase.Refresh();
#endif
        }


    }
}
