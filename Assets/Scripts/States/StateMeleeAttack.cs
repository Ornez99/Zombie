using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMeleeAttack : IState {

    private float distanceToAttack = 1f;

    private Unit unit;

    public StateMeleeAttack(Unit unit) {
        this.unit = unit;
    }

    public int GetScore() {
        Unit closestEnemy = null;//unit.Vision.ClosestEnemy;
        if (closestEnemy != null) {
            if (Vector3.Distance(closestEnemy.transform.position, unit.transform.position) <= distanceToAttack)
                return 125;
        }
        return 0;
    }

    public void OnStateSelected() {
        
    }

    public void Tick() {
        unit.Weapon.Shoot();
    }
}
