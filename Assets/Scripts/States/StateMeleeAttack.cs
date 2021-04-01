using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMeleeAttack : IState {

    private Animator animator = default;
    private Unit unit = default;
    private Unit closestEnemy = default;

    private float attackRadius = default;
    private float timeToEndOfAttackAnimation = default;

    public StateMeleeAttack(Unit unit, Animator animator) {
        this.unit = unit;
        this.animator = animator;
    }

    public int GetScore() {
        closestEnemy = unit.FieldOfView.ClosestEnemy;

        if (timeToEndOfAttackAnimation > 0)
            return 125;

        if (closestEnemy != null) {
            if (Vector3.Distance(closestEnemy.transform.position, unit.transform.position) <= unit.Weapon.AttackRange)
                return 125;
        }

        return 0;
    }

    public void OnStateSelected() {
        timeToEndOfAttackAnimation = 0;
        
        animator.SetBool("MeleeAttack", true);
    }

    public void OnStateDeselected() {
        animator.SetBool("MeleeAttack", false);
    }

    public void Tick() {
        if (timeToEndOfAttackAnimation > 0)
            timeToEndOfAttackAnimation -= Time.deltaTime;
        else {
            IKillable enemyKillable = closestEnemy?.GetComponent<IKillable>();
            if (enemyKillable != null)
                unit.Weapon.AttackUnit(enemyKillable);

            timeToEndOfAttackAnimation = unit.Weapon.TimeBetweenShots;
        }
    }
}
