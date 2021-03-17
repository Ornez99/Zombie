using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SmellManager : MonoBehaviour {

    public static SmellManager Instance;

    public bool ShowSmellMap;
    public bool ShowVectorMap;

    private const int smellSpreadingValue = 100;

    private const float timeToNextSmellUpdate = 0.1f;
    private float lastSmellUpdateTime = 0;

    public int[,] SmellMap { get; set; }
    public Vector2[,] VectorMap { get; set; }

    public void Initialize() {
        if (Instance != null && Instance != this) {
            Debug.LogError("There can be only one instance of this script!");
            Destroy(this);
        }

        Instance = this;
        VectorMap = new Vector2[Map.Instance.MapSize, Map.Instance.MapSize];
        SmellMap = new int[Map.Instance.MapSize - 2, Map.Instance.MapSize - 2];
    }

    private void Update() {
        lastSmellUpdateTime += Time.deltaTime;

        if (lastSmellUpdateTime >= timeToNextSmellUpdate) {
            lastSmellUpdateTime = 0;
            ProcessSmell();
            VectorMap = SmellVectorMapGenerator.GenerateVectorMap(SmellMap);
        }
    }

    private void ProcessSmell() {
        for (int y = 1; y < Map.Instance.MapSize - 1; y++) {
            for (int x = 1; x < Map.Instance.MapSize - 1; x++) {
                Node node = Map.Instance.Grid[x, y];
                if (node.SmellValue >= smellSpreadingValue) {
                    List<Node> neighbours = Map.Instance.GetNeighbours4(node);
                    foreach (Node neightbour in neighbours) {
                        if (neightbour.Smellable && neightbour.SmellValue < node.SmellValue - 3)
                            neightbour.SmellValue = node.SmellValue - 3;
                    }
                }

                if (node.SmellValue > 0)
                    node.SmellValue--;

                SmellMap[x - 1, y - 1] = node.SmellValue;
            }
        }

        /*foreach (Node node in Map.Instance.Grid) {
            if (node.SmellValue >= smellSpreadingValue) {
                List<Node> neighbours = Map.Instance.GetNeighbours4(node);
                foreach (Node neightbour in neighbours) {
                    if (neightbour.Smellable && neightbour.SmellValue < node.SmellValue - 3)
                        neightbour.SmellValue = node.SmellValue - 3;
                }
            }

            if (node.SmellValue > 0)
                node.SmellValue--;

            SmellMap[node.XId, node.YId] = node.SmellValue;
        }*/
    }

#if UNITY_EDITOR
    private void OnDrawGizmos() {
        if (ShowSmellMap) {
            foreach (Node node in Map.Instance.Grid) {
                Handles.Label(node.CenterPos, node.SmellValue.ToString());
            }
        }
        else if (ShowVectorMap) {
            Gizmos.color = Color.red;//new Color(0x2C, 0x6D, 0x51);
            for (int y = 1; y < Map.Instance.MapSize - 1; y++) {
                for (int x = 1; x < Map.Instance.MapSize - 1; x++) {
                    Vector3 center = new Vector3(x + 0.5f, 0, y + 0.5f);
                    Vector3 vectorEnd = new Vector3(Instance.VectorMap[x - 1, y - 1].x / 4f, 0, Instance.VectorMap[x - 1, y - 1].y / 4f);
                    Gizmos.DrawSphere(center, 0.05f);
                    Gizmos.DrawLine(center, center + vectorEnd);
                }
            }
        }
    }
#endif
}
