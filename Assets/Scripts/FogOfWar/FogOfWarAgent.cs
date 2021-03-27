using System.Collections.Generic;
using UnityEngine;

public class FogOfWarAgent : MonoBehaviour {


    private List<Node> nodesInRadius = new List<Node>();
    private FieldOfViewAgent fieldOfViewAgent;

    private void Awake() {
        fieldOfViewAgent = GetComponent<FieldOfViewAgent>();
    }

    private void Start() {
        FogOfWar.Instance.AddAgent(this);
    }

    public void UpdateNodesInRadius() {
        nodesInRadius.Clear();
        nodesInRadius.Add(Map.GetNodeFromPos(transform.position));
        foreach (Vector3 endPos in fieldOfViewAgent.FieldOfViewVectors) {
            float length = (endPos - transform.position).magnitude;
            Vector3 direction = (endPos - transform.position).normalized;
            float step = direction.magnitude;
            float currentLength = step;
            Vector3 currentPos = transform.position + direction;
            while (currentLength <= length) {
                Node candidate = Map.GetNodeFromPos(currentPos);
                if (candidate != null && nodesInRadius.Contains(candidate) == false)
                    nodesInRadius.Add(candidate);

                currentPos += direction;
                currentLength += step;
            } 
        }
    }

    public void SetNodesInRadius(bool visible) {
        foreach (Node node in nodesInRadius) {
            node.Visible = visible;

            if (node.Visited == false)
                node.Visited = true;
        }
    }

}
