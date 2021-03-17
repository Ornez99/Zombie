using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour {

    [SerializeField]
    protected float reloadTime = 0.5f;
    protected float reloadTimer;

    public abstract void Shoot();

}


