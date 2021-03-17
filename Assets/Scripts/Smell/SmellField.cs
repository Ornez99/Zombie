using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SmellField : MonoBehaviour {
    public static int[,] GenerateHeatmap (Node[] startNodes, int startX, int startY, int size) {
        if (startX < 0)
            startX = 0;
        if (startY < 0)
            startY = 0;

        if (startX + size > Map.Instance.MapSize - 1)
            size = (Map.Instance.MapSize - 1) - startX;
        if (startY + size > Map.Instance.MapSize - 1)
            size = (Map.Instance.MapSize - 1) - startY;

        int[,] heatmap = new int[size, size];
        
        for (int y = 0; y < size; y++) {
            for (int x = 0; x < size; x++) {
                heatmap[x, y] = (Map.Instance.Grid[x + startX, y + startY].Smellable) ? 0 : 65535;
            }
        }

        List<Vector3Int> list1 = new List<Vector3Int>(); 
        List<Vector3Int> list2 = new List<Vector3Int>();

        for (int i = 0; i < startNodes.Length; i++) {
            list1.Add(new Vector3Int(startNodes[i].XId - startX, startNodes[i].YId - startY, 0));
        }

        while (list1.Count != 0) {
            for (int i = 0; i < list1.Count; i++) {
                heatmap[list1[i].x, list1[i].y] = list1[i].z;

                if (heatmap[list1[i].x, list1[i].y + 1] == 0)
                    list2.Add(new Vector3Int(list1[i].x, list1[i].y + 1, list1[i].z + 1));
                if (heatmap[list1[i].x + 1, list1[i].y] == 0)
                    list2.Add(new Vector3Int(list1[i].x + 1, list1[i].y, list1[i].z + 1));
                if (heatmap[list1[i].x, list1[i].y - 1] == 0)
                    list2.Add(new Vector3Int(list1[i].x, list1[i].y - 1, list1[i].z + 1));
                if (heatmap[list1[i].x - 1, list1[i].y] == 0)
                    list2.Add(new Vector3Int(list1[i].x - 1, list1[i].y, list1[i].z + 1));
            }

            list2 = list2.Distinct().ToList();
            list1.Clear();
            list1 = new List<Vector3Int>(list2);
            list2.Clear();
        }

        for (int i = 0; i < startNodes.Length; i++) {
            heatmap[startNodes[i].XId - startX, startNodes[i].YId - startY] = 0;
        }

        return heatmap;
    }

    public static Vector2[,] GenerateVectormap(int[,] smellMap) {
        Vector2[,] vectormap = new Vector2[smellMap.GetLength(0), smellMap.GetLength(1)];

        for (int y = 0; y < smellMap.GetLength(1); y++) {
            for (int x = 0; x < smellMap.GetLength(0); x++) {
                int nodeX = 0;
                int nodeY = 0;
                int minValue = 65535;
                for (int yi = -1; yi <= 1; yi++) {
                    for (int xi = -1; xi <= 1; xi++) {
                        if (xi == 0 && yi == 0)
                            continue;

                        if (smellMap[x + xi,y + yi] < minValue) {
                            minValue = smellMap[x + xi,y + yi];
                            nodeX = x + xi;
                            nodeY = y + yi;
                        }
                    }
                }
                vectormap[x,y] = new Vector2(nodeX - x, nodeY - y);
                vectormap[x, y].Normalize();
            }
        }

        return vectormap;
    }
}