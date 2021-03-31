using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateAttackZombies : IState {

    private Unit unit;
    private Unit closestEnemy;

    public StateAttackZombies(Unit unit) {
        this.unit = unit;
    }

    public int GetScore() {
        closestEnemy = unit.FieldOfView.ClosestEnemy;

        if (closestEnemy != null && unit.Weapon != null) {
            return 125;
        }
            

        return 0;
    }

    public void OnStateDeselected() {

    }

    public void OnStateSelected() {
        
    }

    public void Tick() {
        IKillable enemy = closestEnemy.GetComponent<IKillable>();
        if (enemy != null) {
            unit.transform.LookAt(closestEnemy.transform);
            Vector3 currentRotation = unit.transform.rotation.eulerAngles;
            currentRotation = currentRotation += new Vector3(0, Random.Range(-5f, 5f), 0);
            unit.transform.rotation = Quaternion.Euler(currentRotation);
            unit.Weapon.Attack();
        }
    }
}
