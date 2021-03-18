using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pathfinding : MonoBehaviour{

    private Map map;
    public static Pathfinding Instance;

    public void Initialize() {
        if (Instance != null && Instance != this) {
            Debug.LogError("There can be only one instance of this script!");
            Destroy(this);
        }
        map = Map.Instance;
        Instance = this;
    }

    public List<Node> FindPath(Node startNode, Node targetNode) {
        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0) {
            Node currentNode = openSet[0];
            for (int i = 0; i < openSet.Count; i++) {
                if (openSet[i].FCost <= currentNode.FCost && openSet[i].HCost < currentNode.HCost) {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode) {
                return RetracePath(startNode, currentNode);
            }


            foreach (Node neighbour in map.GetNeighbours(currentNode)) {
                if (!neighbour.Walkable || closedSet.Contains(neighbour) || !CheckForWalls(currentNode.XId, currentNode.YId, (currentNode.XId - neighbour.XId) * -1, (currentNode.YId - neighbour.YId) * -1)) {
                    continue;
                }

                int newMovementCostToNeighbour = currentNode.GCost + GetDistance(currentNode, neighbour);
                if (newMovementCostToNeighbour < neighbour.GCost || !openSet.Contains(neighbour)) {
                    neighbour.GCost = newMovementCostToNeighbour;
                    neighbour.HCost = GetDistance(neighbour, targetNode);
                    neighbour.Parent = currentNode;

                    if (!openSet.Contains(neighbour)) openSet.Add(neighbour);
                }
            }
        }

        return new List<Node>();
    }

    public List<Vector3> GetPath(Vector3 startPos, Vector3 targetPos) {
        List<Node> path = FindPath(Map.GetNodeFromPos(startPos), Map.GetNodeFromPos(targetPos));
        List<Vector3> pathInVector3 = new List<Vector3>();

        for (int i = 0; i < path.Count - 1; i++)
            pathInVector3.Add(path[i].CenterPos);

        if (pathInVector3.Count > 0) {
            pathInVector3.RemoveAt(pathInVector3.Count - 1);
            pathInVector3.Add(targetPos);
        }
            
        return pathInVector3;
    }

    private List<Node> RetracePath(Node startNode, Node endNode) {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode) {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }

        path.Reverse();
        return path;
    }
    
    private int GetDistance(Node nodeA, Node nodeB) {
        int dstX = (int)Mathf.Abs(nodeA.CenterPos.x - nodeB.CenterPos.x);
        int dstY = (int)Mathf.Abs(nodeA.CenterPos.z - nodeB.CenterPos.z);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }

    private bool CheckForWalls(int x, int y, int xi, int yi) {
        if (xi == -1 && yi == -1) {
            if (Map.Instance.Grid[x + xi + 1, y + yi].Walkable == false || Map.Instance.Grid[x + xi, y + yi + 1].Walkable == false) {
                return false;
            }
        }
        else if (xi == 1 && yi == -1) {
            if (Map.Instance.Grid[x + xi - 1, y + yi].Walkable == false || Map.Instance.Grid[x + xi, y + yi + 1].Walkable == false) {
                return false;
            }
        }
        else if (xi == -1 && yi == 1) {
            if (Map.Instance.Grid[x + xi + 1, y + yi].Walkable == false || Map.Instance.Grid[x + xi, y + yi - 1].Walkable == false) {
                return false;
            }
        }
        else if (xi == 1 && yi == 1) {
            if (Map.Instance.Grid[x + xi - 1, y + yi].Walkable == false || Map.Instance.Grid[x + xi, y + yi - 1].Walkable == false) {
                return false;
            }
        }
        return true;
    }

}