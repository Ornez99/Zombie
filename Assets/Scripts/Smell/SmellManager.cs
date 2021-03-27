using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SmellManager : MonoBehaviour {

    public static SmellManager Instance;

    [SerializeField]
    private bool showSmellMap;
    [SerializeField]
    private bool showVectorMap;

    [SerializeField]
    private int smellSpreadingValue = 100;
    [SerializeField]
    private int smellSpreadingCost = 3;

    [SerializeField]
    private int updatePerFixedFrames = 5;
    private int currentFixedFramesCounter = 0;

    private Color32 smellColor = new Color32(0x2C, 0x6D, 0x51, 0xFF);
    private List<SmellAgent> agents = new List<SmellAgent>();

    public int[,] SmellMap { get; set; }
    public Vector2[,] VectorMap { get; set; }

    public void Initialize() {
        if (Instance != null && Instance != this) {
            Debug.LogError("There can be only one instance of this script!");
            Destroy(this);
        }

        VectorMap = new Vector2[Map.Instance.MapSize, Map.Instance.MapSize];
        SmellMap = new int[Map.Instance.MapSize, Map.Instance.MapSize];
        Instance = this;
    }

    private void FixedUpdate() {
        if (currentFixedFramesCounter >= updatePerFixedFrames) {
            currentFixedFramesCounter = 0;
            UpdateDataFromAgents();
            ProcessSmell();
            UpdateVectorMap();
        }
        currentFixedFramesCounter++;
    }

    public void AddAgent(SmellAgent agent) {
        if (agents.Contains(agent) == false)
            agents.Add(agent);
    }

    public void RemoveAgent(SmellAgent agent) {
        if (agents.Contains(agent) == true)
            agents.Add(agent);
    }

    private void UpdateDataFromAgents() {
        foreach (SmellAgent agent in agents) {
            agent.UpdateSmell();
        }
    }

    private void ProcessSmell() {
        int smellMapWidth = SmellMap.GetLength(0);
        int smellMapHeight = SmellMap.GetLength(1);

        for (int y = 0; y < smellMapHeight; y++) {
            for (int x = 0; x < smellMapWidth; x++) {
                if (SmellMap[x,y] >= smellSpreadingValue) {
                    if (y + 1 < smellMapHeight)
                        if (Map.Instance.Grid[x, y + 1]?.Smellable == true && SmellMap[x, y + 1] < SmellMap[x, y] - smellSpreadingCost)
                            SmellMap[x, y + 1] = SmellMap[x, y] - smellSpreadingCost;
                    if (x + 1 < smellMapWidth)
                        if (Map.Instance.Grid[x + 1, y]?.Smellable == true && SmellMap[x + 1, y] < SmellMap[x, y] - smellSpreadingCost)
                            SmellMap[x + 1, y] = SmellMap[x, y] - smellSpreadingCost;
                    if (y - 1 >= 0)
                        if (Map.Instance.Grid[x, y - 1]?.Smellable == true && SmellMap[x, y - 1] < SmellMap[x, y] - smellSpreadingCost)
                            SmellMap[x, y - 1] = SmellMap[x, y] - smellSpreadingCost;
                    if (x - 1 >= 0)
                        if (Map.Instance.Grid[x - 1, y]?.Smellable == true && SmellMap[x - 1, y] < SmellMap[x, y] - smellSpreadingCost)
                            SmellMap[x - 1, y] = SmellMap[x, y] - smellSpreadingCost;
                }

                if (SmellMap[x, y] > 0)
                    SmellMap[x, y]--;
            }
        }
    }

    private void UpdateVectorMap() {
        VectorMap = SmellVectorMapGenerator.GenerateVectorMap(SmellMap);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos() {
        Gizmos.color = smellColor;
        if (showSmellMap) {
            foreach (Node node in Map.Instance.Grid) {
                //Handles.Label(node.CenterPos, node.SmellValue.ToString());
                Handles.Label(node.CenterPos, SmellMap[node.XId, node.YId].ToString());
            }
        }
        else if (showVectorMap) {
            for (int y = 0; y < Map.Instance.MapSize; y++) {
                for (int x = 0; x < Map.Instance.MapSize; x++) {
                    Vector3 center = new Vector3(x + 0.5f, 0, y + 0.5f);
                    Vector3 vectorEnd = new Vector3(Instance.VectorMap[x, y].x / 4f, 0, Instance.VectorMap[x, y].y / 4f);
                    Gizmos.DrawWireCube(center, Vector3.one * 0.05f);
                    Gizmos.DrawLine(center, center + vectorEnd);
                }
            }
        }
    }
#endif
}
