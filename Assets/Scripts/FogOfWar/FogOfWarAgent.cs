using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWarAgent : MonoBehaviour {

    [SerializeField]
    private float radius = default;
    private List<Node> nodesInRadius = new List<Node>();

    private void Start() {
        FogOfWar.Instance.AddAgent(this);
    }

    public void UpdateNodesInRadius() {
        nodesInRadius = Map.GetNodesInRadius(radius, Map.GetNodeFromPos(transform.position));   
    }

    public void SetNodesInRadius(bool visible) {
        foreach (Node node in nodesInRadius) {
            node.Visible = visible;

            if (node.Visited == false)
                node.Visited = true;
        }
    }

}
