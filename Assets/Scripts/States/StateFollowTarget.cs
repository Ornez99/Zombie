using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateFollowTarget : IState {

    private Unit unit;
    private Unit target;
    private Animator animator;
    private Node lastTargetNode;

    public StateFollowTarget(Unit unit, Animator animator) {
        this.unit = unit;
        this.animator = animator;
    }

    public int GetScore() {
        if (target == null)
            return 0;

        int score = (int)Vector3.Distance(unit.transform.position, target.transform.position) * 10 - 40;
        score = Mathf.Min(score, 75);
        return score;
    }

    public void OnStateDeselected() {
        animator.SetBool("Run", false);
        unit.Drive.ResetPath();
    }

    public void OnStateSelected() {
        animator.SetBool("Run", true);
        lastTargetNode = target.Node;
        unit.Drive.CreateAndSetPathToPosition(target.transform.position);
        unit.Drive.Speed = unit.UnitData.MovementSpeedRun;
    }

    public void SetTarget(Unit target) {
        this.target = target;
    }

    public void Tick() {
        if (lastTargetNode != target.Node) {
            lastTargetNode = target.Node;
            unit.Drive.CreateAndSetPathToPosition(target.transform.position);
        }

        unit.Drive.MoveWithPath();
        if (unit.Drive.DestinationReached)
            lastTargetNode = null;
    }
}
