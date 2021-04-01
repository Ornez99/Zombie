using UnityEngine;

public class Node {

    public bool Walkable;
    public bool Buildable;
    public bool Viewable;
    public bool Smellable;

    public int SmellValue;

    public Vector3 Pos;
    public Vector3 CenterPos;

    public int XId, YId;

    public Node Parent;
    public int GCost;
    public int HCost;
    public int FCost {
        get {
            return GCost + HCost;
        }
    }

    public Building Building { get; set; }

    public bool Visited = false;
    public bool Visible = false;

    public Node(bool walkable, bool buildable, Vector3 pos, int xId, int yId) {
        Walkable = walkable;
        Buildable = buildable;
        Viewable = true;
        Smellable = true;
        SmellValue = 0;
        Pos = pos;
        CenterPos = new Vector3(pos.x + 0.5f, pos.y, pos.z + 0.5f);
        XId = xId;
        YId = yId;
    }

    public Vector3 GetRandomPosOnNode() {
        float x = Pos.x + Random.Range(0f, 0.999f);
        float z = Pos.z + Random.Range(0f, 0.999f);
        return new Vector3(x, Pos.y, z);
    }
}


