using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMoveToEnemy : IState {

    private Unit unit;
    private Unit enemyUnit;
    private Node lastTargetNode;
    
    public StateMoveToEnemy(Unit unit) {
        this.unit = unit;
    }

    public int GetScore() {
        int value = lastTargetNode != null ? 80 : 0;
        value = unit.Vision.EnemySpotted ? 100 : value;
        return value;
    }

    public void Tick() {
        unit.Drive.SetSpeed(4f);
        enemyUnit = unit.Vision.ClosestEnemy;

        if (lastTargetNode == null) {
            if (enemyUnit != null) {
                lastTargetNode = enemyUnit.Drive.Node;
                unit.Drive.CreatePathToPosition(enemyUnit.transform.position);
            }
        }

        if (enemyUnit != null) {
            if (lastTargetNode != enemyUnit.Drive.Node) {
                lastTargetNode = enemyUnit.Drive.Node;
                unit.Drive.CreatePathToPosition(enemyUnit.transform.position);
            }
        }

        unit.Drive.Move();
        if (unit.Drive.DestinationReached)
            lastTargetNode = null;
    }
}
