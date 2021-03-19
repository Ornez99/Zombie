using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateIdle : IState {

    private Unit unit;
    private float timeToNextMove;

    public StateIdle(Unit unit) {
        this.unit = unit;
    }

    public int GetScore() {
        return 1;    
    }

    public void OnStateSelected() {
    }

    public void Tick() {
        timeToNextMove -= Time.deltaTime;
        unit.Drive.MoveWithPath();
        if (timeToNextMove <= 0) {
            timeToNextMove = Random.Range(3f, 6f);
            SetNewRandomDestination();
        }
    }

    private void SetNewRandomDestination() {
        Node node = Map.GetNodeFromPos(unit.transform.position);
        List<Node> nodesNearUnit = Map.GetNodesInRadius(3f, node);
        node = Map.GetRandomWalkableNode(nodesNearUnit);
        unit.Drive.CreateAndSetPathToPosition(node.CenterPos);
    }

}
