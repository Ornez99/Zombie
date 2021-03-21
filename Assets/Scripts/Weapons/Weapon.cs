using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour {

    [SerializeField]
    protected float reloadTime = 0.5f;
    protected float reloadTimer;
    [SerializeField]
    protected float attackRange;

    public abstract void Shoot();
    public abstract void MeleeAttack(Unit target);
}


