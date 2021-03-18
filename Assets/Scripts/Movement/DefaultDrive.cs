using System.Collections.Generic;
using UnityEngine;

public class DefaultDrive : IMoveable {

    private const float minimumDistance = 0.1f;

    private Transform owner;
    private float movementSpeed;
    private Vector3 destination;
    private List<Vector3> path;

    public Node Node { get; private set; }
    public bool NodeChanged { get; private set; }
    public bool PathFound { get; private set; }
    public bool DestinationReached {
        get {
            if (PathFound == false)
                return false;

            if (Vector3.Distance(owner.position, destination) <= minimumDistance)
                return true;

            return false;
        }
    }

    public DefaultDrive(float movementSpeed, Transform owner) {
        this.movementSpeed = movementSpeed;
        this.owner = owner;
        PathFound = false;
        Node = Map.GetNodeFromPos(owner.position);
    }

    public void CreatePathToPosition(Vector3 pos) {
        PathFound = false;
        path = Pathfinding.Instance.GetPath(owner.position, pos);
        SetDestinationAndPathFound();
    }

    public void Move() {
        if (PathFound == false)
            return;

        if (DestinationReached == false) {
            if (path.Count == 0)
                return;

            //TEST
            Vector3 lastPos = owner.position;
            for (int i = 0; i < path.Count; i++) {
                Debug.DrawLine(lastPos + Vector3.up, path[i] + Vector3.up, Color.black);
                lastPos = path[i];
            }


            Vector3 direction = -1 * Vector3.Normalize(owner.position - path[0]);
            Translate(direction);
            
            if (Vector3.Distance(owner.position, path[0]) < minimumDistance)
                path.RemoveAt(0);
            
        }
    }

    public void Translate(Vector3 normalVector3) {
        Vector3 potentialPositionV3 = owner.position + (normalVector3 * movementSpeed * Time.deltaTime);
        Vector2 potentialPosition = new Vector2(potentialPositionV3.x, potentialPositionV3.z);
        Vector2 vCurrentCell = new Vector2(Node.Pos.x, Node.Pos.z);
        Vector2 vTargetCell = potentialPosition;

        Vector2 vAreaTL = vCurrentCell + new Vector2(-2, 2);
        Vector2 vAreaBR = vCurrentCell + new Vector2(2, -2);

        Vector2 vCell = new Vector2();
        for (vCell.y = vAreaBR.y; vCell.y <= vAreaTL.y; vCell.y++) {
            for (vCell.x = vAreaTL.x; vCell.x <= vAreaBR.x; vCell.x++) {
                Node n = Map.GetNodeFromPos(new Vector3(vCell.x, 0, vCell.y));
                if (n == null)
                    continue;
                if (n.Walkable == false) {
                    Vector2 nearestPoint;
                    nearestPoint.x = Mathf.Max((float)(vCell.x), Mathf.Min(potentialPosition.x, (float)(vCell.x + 1)));
                    nearestPoint.y = Mathf.Max((float)(vCell.y), Mathf.Min(potentialPosition.y, (float)(vCell.y + 1)));

                    Vector2 rayToNearest = nearestPoint - potentialPosition;
                    float overlap = owner.GetComponent<CapsuleCollider>().radius - rayToNearest.magnitude;

                    if (float.IsNaN(overlap))
                        overlap = 0;

                    if (overlap > 0) {
                        potentialPosition = potentialPosition - rayToNearest.normalized * overlap;
                        potentialPositionV3 = new Vector3(potentialPosition.x, 0, potentialPosition.y);
                    }
                }
            }
        }
        owner.LookAt(potentialPositionV3);
        owner.position = potentialPositionV3;
        SetCurrentNode();
    }

    private void SetDestinationAndPathFound() {
        if (path.Count > 0) {
            destination = path[path.Count - 1];
            PathFound = true;
        }
        else {
            destination = owner.position;
            PathFound = false;
        }
    }

    private void SetCurrentNode() {
        Node node = Map.GetNodeFromPos(owner.position);
        //if (Vector3.Distance(owner.transform.position, Node.CenterPos) > 0.5f + owner.GetComponent<CapsuleCollider>().radius) {
        if (Vector3.Distance(owner.transform.position, Node.CenterPos) > 0.5f) {
            NodeChanged = true;
            Node = node;
        }
        else
            NodeChanged = false;
    }
}