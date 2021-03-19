using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateIdle : IState {

    private Unit unit;
    private float timeToNextMove;
    private Animator animator;

    public StateIdle(Unit unit, Animator animator) {
        this.unit = unit;
        this.animator = animator;
    }

    public int GetScore() {
        return 1;
    }

    public void OnStateDeselected() {
        animator.SetBool("Moving", false);
    }

    public void OnStateSelected() {
        
    }

    public void Tick() {
        timeToNextMove -= Time.deltaTime;
        unit.Drive.MoveWithPath();
        if (unit.Drive.DestinationReached) {
            animator.SetBool("Moving", false);
        }

        if (timeToNextMove <= 0) {
            timeToNextMove = Random.Range(3f, 6f);
            SetNewRandomDestination();
        }
    }

    private void SetNewRandomDestination() {
        animator.SetBool("Moving", true);
        Node node = Map.GetNodeFromPos(unit.transform.position);
        List<Node> nodesNearUnit = Map.GetNodesInRadius(3f, node);
        node = Map.GetRandomWalkableNode(nodesNearUnit);
        unit.Drive.CreateAndSetPathToPosition(node.CenterPos);
    }

}
