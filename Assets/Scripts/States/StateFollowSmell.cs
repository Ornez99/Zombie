using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateFollowSmell : IState {

    private Unit unit;

    public StateFollowSmell(Unit unit) {
        this.unit = unit;
    }

    public int GetScore() {
        if (unit.Node == null)
            Debug.Log("IS NULL");


        return SmellManager.Instance.SmellMap[unit.Node.XId, unit.Node.YId] > 0 ? 90 : 0;
    }

    public void OnStateSelected() {
        unit.Drive.SetSpeed(2f);
    }

    public void Tick() {
        Vector3 moveDirection = Vector2Methods.ToXZ(SmellManager.Instance.VectorMap[unit.Node.XId, unit.Node.YId]);
        unit.Drive.MoveWithNormalizedDirection(moveDirection);
    }
}
