using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour {

    [SerializeField]
    protected float damage;
    [SerializeField]
    protected float timeBetweenShots;
    [SerializeField]
    protected float effectsDisplayTime;
    protected float timer;

    public abstract void Attack();
    public abstract void AttackUnit(IKillable target);
}


