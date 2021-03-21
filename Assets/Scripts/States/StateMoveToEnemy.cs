using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMoveToEnemy : IState {

    private Unit unit;
    private Unit enemyUnit;
    private Node lastTargetNode;
    private Animator animator;
    
    public StateMoveToEnemy(Unit unit, Animator animator) {
        this.unit = unit;
        this.animator = animator;
    }

    public int GetScore() {
        int value = lastTargetNode != null ? 80 : 0;
        value = (unit.VisionInterpreter.ClosestEnemy != null) ? 100 : value;
        return value;
    }

    public void OnStateDeselected() {
        animator.SetBool("Run", false);
    }

    public void OnStateSelected() {
        unit.Drive.SetSpeed(4f);
        animator.SetBool("Run", true);
    }

    public void Tick() {
        enemyUnit = unit.VisionInterpreter.ClosestEnemy;

        if (lastTargetNode == null) {
            if (enemyUnit != null) {
                lastTargetNode = enemyUnit.Node;
                unit.Drive.CreateAndSetPathToPosition(enemyUnit.transform.position);
            }
        }

        if (enemyUnit != null) {
            if (lastTargetNode != enemyUnit.Node) {
                lastTargetNode = enemyUnit.Node;
                unit.Drive.CreateAndSetPathToPosition(enemyUnit.transform.position);
            }
        }

        unit.Drive.MoveWithPath();
        if (unit.Drive.DestinationReached)
            lastTargetNode = null;
    }



}
