using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Pistol : Weapon {

    [SerializeField]
    private Transform bulletSpawnTransform = null;
    [SerializeField]
    private GameObject bullet = null;
    [SerializeField]
    private float fireForce = 1000;

    private void Update() {
        if (reloadTimer > 0)
            reloadTimer -= Time.deltaTime;
    }

    public override void Shoot() {
        if (reloadTimer <= 0) {
            Rigidbody spawnedBullet = Instantiate(bullet, bulletSpawnTransform.position, Quaternion.Euler(0, bulletSpawnTransform.rotation.eulerAngles.y, 0)).GetComponent<Rigidbody>();
            spawnedBullet.AddForce(spawnedBullet.transform.forward * fireForce);
            reloadTimer = reloadTime;
            Destroy(spawnedBullet.gameObject, 5f);
        }
    }

}
