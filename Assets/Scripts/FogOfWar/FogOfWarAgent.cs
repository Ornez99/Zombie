using System.Collections.Generic;
using UnityEngine;

public class FogOfWarAgent : MonoBehaviour
{
    private List<Node> nodesInRadius = new List<Node>();

    private Unit unit;

    private void Start()
    {
        unit = GetComponent<Unit>();
        FogOfWar.Instance.AddAgent(this);
    }

    public void UpdateNodesInRadius() {
        nodesInRadius.Clear();
        
        foreach (Vector3 endPos in unit.FieldOfView.Vectors)
        {
            if (endPos == Vector3.zero)
                break;

            float length = (new Vector3(endPos.x, 0, endPos.z) - new Vector3(transform.position.x, 0, transform.position.z)).magnitude;
            Vector3 direction = (new Vector3(endPos.x, 0, endPos.z) - new Vector3(transform.position.x, 0, transform.position.z)).normalized;
            float step = direction.magnitude;
            float currentLength = step;
            Vector3 currentPos = transform.position + direction;
            while (currentLength < length)
            {
                Node candidate = Map.GetNodeFromPos(currentPos);
                if (candidate.Viewable == false)
                    break;

                if (candidate != null && nodesInRadius.Contains(candidate) == false)
                    nodesInRadius.Add(candidate);

                currentPos += direction;
                currentLength += step;
            } 
        }

        Node currentNode = Map.GetNodeFromPos(transform.position);
        for (int y = -1; y <= 0; y++)
        {
            for (int x = -1; x <= 0; x++)
            {
                Node candidate = Map.Instance.Grid[currentNode.XId + x, currentNode.YId + y];
                if (candidate.Viewable)
                    if (nodesInRadius.Contains(candidate) == false)
                        nodesInRadius.Add(candidate);
            }
        }
    }

    public void SetNodesInRadius(bool visible)
    {
        foreach (Node node in nodesInRadius)
        {
            node.Visible = visible;

            if (node.Visited == false)
                node.Visited = true;
        }
    }

    private void OnDestroy()
    {
        FogOfWar.Instance.RemoveAgent(this);
    }
}
