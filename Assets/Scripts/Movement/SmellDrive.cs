using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmellDrive : IMoveable {

    private Transform owner;
    private float movementSpeed;

    public Node Node { get; private set; }
    public bool NodeChanged { get; private set; }

    public bool PathFound => throw new System.NotImplementedException();

    public bool DestinationReached => throw new System.NotImplementedException();

    public SmellDrive(float movementSpeed, Transform owner) {
        this.owner = owner;
        this.movementSpeed = movementSpeed;
        Node = Map.GetNodeFromPos(owner.position);
    }

    public void CreatePathToPosition(Vector3 position) {
        throw new System.NotImplementedException();
    }

    public void Move() {
        throw new System.NotImplementedException();
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
                Node node = Map.GetNodeFromPos(new Vector3(Mathf.FloorToInt(vCell.x),0, Mathf.FloorToInt(vCell.y)));
                if (node?.Walkable == false) {
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
        owner.position = potentialPositionV3;
        SetCurrentNode();
    }

    private void SetCurrentNode() {
        /*Node node = Map.GetNodeFromPos(owner.position);
        if (node != Node) {
            NodeChanged = true;
            Node = node;
        }
        else
            NodeChanged = false;*/
        Node node = Map.GetNodeFromPos(owner.position);
        if (Vector3.Distance(owner.transform.position, Node.CenterPos) > 0.5f + owner.GetComponent<CapsuleCollider>().radius) {
            NodeChanged = true;
            Node = node;
        }
        else
            NodeChanged = false;




    }
}
