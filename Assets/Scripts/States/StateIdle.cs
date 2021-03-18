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

    public void Tick() {
        timeToNextMove -= Time.deltaTime;
        unit.Drive.Move();
        if (timeToNextMove <= 0) {
            timeToNextMove = Random.Range(3f, 6f);
            SetNewRandomDestination();
        }
    }

    private void SetNewRandomDestination() {
        List<Node> nodesNearUnit = Map.GetNodesInRadius(3f, unit.Drive.Node);
        Node node = Map.GetRandomWalkableNode(nodesNearUnit);
        unit.Drive.CreatePathToPosition(node.GetRandomPosOnNode());
    }

}
