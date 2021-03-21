using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMeleeAttack : IState {

    private float distanceToAttack = 1f;
    private Animator animator;
    private Unit unit;
    private float isAttacking;

    public StateMeleeAttack(Unit unit, Animator animator) {
        this.unit = unit;
        this.animator = animator;
    }

    public int GetScore() {
        Unit closestEnemy = unit.VisionInterpreter.ClosestEnemy;//unit.Vision.ClosestEnemy;

        if (isAttacking > 0)
            return 125;

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
        isAttacking = 0.5f;
    }

    public void Tick() {
        if (isAttacking > 0)
            isAttacking -= Time.deltaTime;

        Unit closestEnemy = unit.VisionInterpreter.ClosestEnemy;
        if (closestEnemy != null)
            if (Vector3.Distance(closestEnemy.transform.position, unit.transform.position) <= distanceToAttack)
                unit.Weapon.AttackUnit(closestEnemy);
    }
}
