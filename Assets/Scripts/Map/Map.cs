using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

    public static Map Instance;

    [SerializeField]
    private int mapSize = 64;

    public int MapSize { get => mapSize; private set => mapSize = value; }
    public Node[,] Grid { get; private set; }

    public GameObject ground;

    public static Node GetNodeFromPos(Vector3 position) {
        if (position.x < 0 || position.x >= Instance.MapSize || position.z < 0 || position.z >= Instance.MapSize)
            return null;

        return Instance.Grid[Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.z)];
    }

    public static Node GetRandomWalkableNode(List<Node> nodes) {
        while (nodes.Count > 0) {
            int randomId = Random.Range(0, nodes.Count);
            Node randomNode = nodes[randomId];
            if (randomNode.Walkable == true)
                return randomNode;
            else
                nodes.RemoveAt(randomId);
        }
        
        return null;
    }

    public static List<Node> GetNodesInRadius(float radius, Node node) {
        List<Node> nodesWithinRadius = new List<Node>();
        int minX = Mathf.FloorToInt(node.Pos.x - radius);
        int maxX = Mathf.CeilToInt(node.Pos.x + radius);
        int minY = Mathf.FloorToInt(node.Pos.z - radius);
        int maxY = Mathf.CeilToInt(node.Pos.z + radius);
        for (int y = minY; y < maxY; y++) {
            for (int x = minX; x < maxX; x++) {
                Node currentNode = GetNodeFromPos(new Vector3(x, 0, y));
                if (currentNode == null)
                    continue;

                float dist = Mathf.Sqrt(Mathf.Pow((currentNode.CenterPos.x - node.CenterPos.x), 2) + Mathf.Pow((currentNode.CenterPos.z - node.CenterPos.z), 2));
                if (dist <= radius) {
                    nodesWithinRadius.Add(currentNode);
                }
            }
        }
        return nodesWithinRadius;
    }

    public void Initialize() {
        if (Instance != null && Instance != this) {
            Debug.LogError("There can be only one instance of this script!");
            Destroy(this);
        }
        Instance = this;
        GenerateGrid();
        GetComponent<MapGenerator>().GenerateMap();

        //mapLoader.LoadDefaultMap();
        
        //Texture2D groundTexture = GenerateGroundTexture2D();
        //ground.GetComponent<Renderer>().material.SetTexture("_Control", groundTexture);
    }

    public List<Node> GetNeighbours(Node node) {
        List<Node> neighbours = new List<Node>();

        for (int z = -1; z <= 1; z++) {
            for (int x = -1; x <= 1; x++) {
                if (x == 0 && z == 0)
                    continue;

                int checkX = node.XId + x;
                int checkZ = node.YId + z;

                neighbours.Add(Grid[checkX, checkZ]);
            }
        }

        return neighbours;
    }

    public List<Node> GetNeighbours4(Node node) {
        List<Node> neighbours4 = new List<Node>();
        neighbours4.Add(Grid[node.XId, node.YId + 1]);
        neighbours4.Add(Grid[node.XId + 1, node.YId]);
        neighbours4.Add(Grid[node.XId, node.YId - 1]);
        neighbours4.Add(Grid[node.XId - 1, node.YId]);

        return neighbours4;
    }

    private void GenerateGrid() {
        Grid = new Node[MapSize, MapSize];

        for (int y = 0; y < MapSize; y++) {
            for (int x = 0; x < MapSize; x++) {
                bool walkable = (x == 0 || y == 0 || x == MapSize - 1 || y == MapSize - 1) ? false : true;
                bool buildable = (x == 0 || y == 0 || x == MapSize - 1 || y == MapSize - 1) ? false : true;
                Vector3 pos = new Vector3(x, 0, y);

                Grid[x, y] = new Node(walkable, buildable, pos, x, y);
            }
        }
    }
}
