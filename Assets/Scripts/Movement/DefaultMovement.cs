using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultMovement : MonoBehaviour, IMoveable {

    [SerializeField]
    private float characterRadius = 0.5f;

    [SerializeField]
    private float minimumDistance = 0.1f;
    [SerializeField]
    private float movementSpeed = 1f;
    private Vector3 destination;
    private List<Vector3> path;

    public bool DestinationReached { get; private set; }
    public bool PathCreated { get; private set; }

    private void Awake() {
        PathCreated = false;
        DestinationReached = true;
    }

    public void CreateAndSetPathToPosition(Vector3 position) {
        destination = position;
        path = Pathfinding.Instance.GetPath(transform.position, destination);
        PathCreated = (path == null || path?.Count == 0) ? false : true;
        DestinationReached = (path == null || path?.Count == 0) ? true : false;
    }

    public void MoveWithPath() {
        if (DestinationReached == true || PathCreated == false)
            return;

        if (path.Count == 0)
            Debug.Log("JAK?");

        Vector3 direction = -1 * Vector3.Normalize(transform.position - path[0]);
        MoveWithNormalizedDirection(direction);
        if (Vector3.Distance(transform.position, path[0]) <= minimumDistance) {
            path.RemoveAt(0);
            if (path.Count == 0)
                DestinationReached = true;
        }
    }

    public void MoveWithNormalizedDirection(Vector3 normalizedDirection) {
        Vector3 potentialPositionV3 = transform.position + (normalizedDirection * movementSpeed * Time.deltaTime);
        Vector2 potentialPositionV2 = new Vector2(potentialPositionV3.x, potentialPositionV3.z);
        Vector2 v2CurrentCell = new Vector2(Map.GetNodeFromPos(transform.position).XId, Map.GetNodeFromPos(transform.position).YId);
        Vector2 v2TargetCell = potentialPositionV2;

        Vector2 vAreaTL = v2CurrentCell + new Vector2(-2, 2);
        Vector2 vAreaBR = v2CurrentCell + new Vector2(2, -2);

        Vector2 vCell = new Vector2();
        for (vCell.y = vAreaBR.y; vCell.y <= vAreaTL.y; vCell.y++) {
            for (vCell.x = vAreaTL.x; vCell.x <= vAreaBR.x; vCell.x++) {
                Node n = Map.GetNodeFromPos(new Vector3(vCell.x, 0, vCell.y));
                if (n == null)
                    continue;
                if (n.Walkable == false) {
                    Vector2 nearestPoint;
                    nearestPoint.x = Mathf.Max((float)(vCell.x), Mathf.Min(potentialPositionV2.x, (float)(vCell.x + 1)));
                    nearestPoint.y = Mathf.Max((float)(vCell.y), Mathf.Min(potentialPositionV2.y, (float)(vCell.y + 1)));

                    Vector2 rayToNearest = nearestPoint - potentialPositionV2;
                    float overlap = characterRadius - rayToNearest.magnitude;

                    if (float.IsNaN(overlap))
                        overlap = 0;

                    if (overlap > 0) {
                        potentialPositionV2 = potentialPositionV2 - rayToNearest.normalized * overlap;
                        potentialPositionV3 = new Vector3(potentialPositionV2.x, 0, potentialPositionV2.y);
                    }
                }
            }
        }

        transform.LookAt(potentialPositionV3);
        transform.position = potentialPositionV3;
    }

    public void SetSpeed(float newSpeed) {
        movementSpeed = newSpeed;
    }
}
