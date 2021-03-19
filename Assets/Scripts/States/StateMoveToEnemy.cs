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
        value = (unit.Vision.GetObjectsInSightWithName("Human(Clone)").Count > 0) ? 100 : value;
        return value;
    }

    public void OnStateDeselected() {
        animator.SetBool("MoveToEnemy", false);
    }

    public void OnStateSelected() {
        unit.Drive.SetSpeed(4f);
        animator.SetBool("MoveToEnemy", true);
    }

    public void Tick() {
        enemyUnit = null;
        float minDist = float.MaxValue;

        foreach (GameObject obj in unit.Vision.GetObjectsInSightWithName("Human(Clone)")) {
            if (obj.GetComponent<Unit>().GetTeam != unit.GetTeam) {
                float potentialDist = Vector3.Distance(obj.transform.position, unit.transform.position);
                if (potentialDist < minDist) {
                    enemyUnit = obj.GetComponent<Unit>();
                    minDist = potentialDist;
                }
            }
        }
        

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
