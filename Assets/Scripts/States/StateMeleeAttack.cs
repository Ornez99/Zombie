using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMeleeAttack : IState {

    private float distanceToAttack = 1f;
    private Animator animator;
    private Unit unit;

    public StateMeleeAttack(Unit unit, Animator animator) {
        this.unit = unit;
        this.animator = animator;
    }

    public int GetScore() {
        Unit closestEnemy = unit.VisionInterpreter.ClosestEnemy;//unit.Vision.ClosestEnemy;

        if (closestEnemy != null) {
            if (Vector3.Distance(closestEnemy.transform.position, unit.transform.position) <= distanceToAttack)
                return 125;
        }
        return 0;
    }

    public void OnStateDeselected() {
        animator.SetBool("MeleeAttack", false);
    }

    public void OnStateSelected() {
        animator.SetBool("MeleeAttack", true);
    }

    public void Tick() {
        unit.Weapon.Shoot();
    }
}
