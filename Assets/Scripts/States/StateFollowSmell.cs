using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateFollowSmell : IState {

    private Unit unit;

    public StateFollowSmell(Unit unit) {
        this.unit = unit;
    }

    public int GetScore() {
        if (SmellManager.Instance.SmellMap[unit.Drive.Node.XId, unit.Drive.Node.YId] > 0)
            return 90;

        return 0;
    }

    public void Tick() {
        Vector2 vector = SmellManager.Instance.VectorMap[unit.Drive.Node.XId, unit.Drive.Node.YId];
        unit.Drive.Translate(new Vector3(vector.x, 0, vector.y));
    }
}
