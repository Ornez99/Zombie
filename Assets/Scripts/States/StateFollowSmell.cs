using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateFollowSmell : IState {

    private Unit unit;
    private Animator animator;

    public StateFollowSmell(Unit unit, Animator animator) {
        this.animator = animator;
        this.unit = unit;
    }

    public int GetScore() {
        return SmellManager.Instance.SmellMap[unit.Node.XId, unit.Node.YId] > 0 ? 90 : 0;
    }

    public void OnStateDeselected() {
        animator.SetBool("Walk", false);
    }

    public void OnStateSelected() {
        unit.Drive.Speed = unit.UnitData.MovementSpeedWalk;
        animator.SetBool("Walk", true);
    }

    public void Tick() {
        Vector3 moveDirection = Vector2Methods.ToXZ(SmellManager.Instance.VectorMap[unit.Node.XId, unit.Node.YId]);
        unit.Drive.MoveWithNormalizedDirection(moveDirection);
    }
}
