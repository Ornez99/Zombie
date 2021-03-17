using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmellVectorMapGenerator {

    public static Vector2[,] GenerateVectorMap(int[,] smellMap) {
        Vector2[,] vectorMap = new Vector2[smellMap.GetLength(0), smellMap.GetLength(1)];

        for (int y = 0; y < smellMap.GetLength(1); y++) {
            for (int x = 0; x < smellMap.GetLength(0); x++) {
                if (x == 0 || y == 0 || x == smellMap.GetLength(0) - 1 || y == smellMap.GetLength(1) - 1)
                    vectorMap[x, y] = Vector2.zero;
                else {
                    int nodeX = 0;
                    int nodeY = 0;
                    int maxValue = 0;
                    for (int yi = -1; yi <= 1; yi++) {
                        for (int xi = -1; xi <= 1; xi++) {
                            if (xi == 0 && yi == 0)
                                continue;

                            if (smellMap[x + xi,y + yi] > maxValue) {
                                /*
                                if (xi == -1 && yi == -1 && (Map.Instance.Grid[x + xi + 1 + 1, y + yi + 1].Walkable == false || Map.Instance.Grid[x + xi + 1 , y + yi + 1 + 1].Walkable == false))
                                    continue;
                                if (xi == 1 && yi == -1 && (Map.Instance.Grid[x + xi + 1 - 1, y + yi + 1].Walkable == false || Map.Instance.Grid[x + xi + 1, y + yi + 1 + 1].Walkable == false))
                                    continue;
                                if (xi == -1 && yi == 1 && (Map.Instance.Grid[x + xi + 1 + 1, y + yi + 1].Walkable == false || Map.Instance.Grid[x + xi + 1, y + yi + 1 - 1].Walkable == false))
                                    continue;
                                if (xi == 1 && yi == 1 && (Map.Instance.Grid[x + xi + 1 - 1, y + yi + 1].Walkable == false || Map.Instance.Grid[x + xi + 1, y + yi + 1 - 1].Walkable == false))
                                    continue;
                                */
                                if (xi == -1 && yi == -1) {
                                    if (Map.Instance.Grid[x + xi + 1 + 1, y + yi + 1].Walkable == false || Map.Instance.Grid[x + xi + 1, y + yi + 1 + 1].Walkable == false) {
                                        continue;
                                    }
                                }
                                else if (xi == 1 && yi == -1) {
                                    if (Map.Instance.Grid[x + xi + 1 - 1, y + yi + 1].Walkable == false || Map.Instance.Grid[x + xi + 1, y + yi + 1 + 1].Walkable == false) {
                                        continue;
                                    }
                                }
                                else if (xi == -1 && yi == 1) {
                                    if (Map.Instance.Grid[x + xi + 1 + 1, y + yi + 1].Walkable == false || Map.Instance.Grid[x + xi + 1, y + yi + 1 - 1].Walkable == false) {
                                        continue;
                                    }
                                }
                                else if (xi == 1 && yi == 1) {
                                    if (Map.Instance.Grid[x + xi + 1 - 1, y + yi + 1].Walkable == false || Map.Instance.Grid[x + xi + 1, y + yi + 1 - 1].Walkable == false) {
                                        continue;
                                    }
                                }

                                maxValue = smellMap[x + xi, y + yi];
                                nodeX = x + xi;
                                nodeY = y + yi;
                            }
                        }
                    }
                    if (nodeX == 0 && nodeY == 0)
                        vectorMap[x, y] = Vector2.zero;
                    else
                        vectorMap[x, y] = new Vector2(nodeX - x, nodeY - y);
                }
            }
        }

        return vectorMap;
    }
}
