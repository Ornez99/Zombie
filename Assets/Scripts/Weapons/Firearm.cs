using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firearm : Weapon {

    [SerializeField]
    private Transform bulletSpawnTransform = null;
    [SerializeField]
    private float shootRange = 0;

    [SerializeField]
    private ParticleSystem hitParticles = null;

    [SerializeField]
    private LineRenderer gunLine = null;
    [SerializeField]
    private LineRenderer gunLaser = null;
    [SerializeField]
    private Light gunLight = null;
    [SerializeField]
    private AudioSource gunAudio = null;
    private int layerShootable = 1 << 9;

    private void Awake() {
        //DisableEffects();
        //gunAudio.Stop();
    }

    private void Update() {
        timer += Time.deltaTime;
        if (timer >= timeBetweenShots * effectsDisplayTime)
            DisableEffects();

        UpdateLaser();
    }

    public override void Attack() {
        if (timer >= timeBetweenShots)
            Shoot();
    }

    public override void AttackUnit(IKillable target) {
        
    }

    private void Shoot() {
        timer = 0;
        gunAudio.Play();
        gunLight.enabled = true;
        // Particles?

        gunLine.enabled = true;
        gunLine.SetPosition(0, bulletSpawnTransform.position);

        Ray shootRay = new Ray();
        shootRay.origin = bulletSpawnTransform.position;
        shootRay.direction = transform.forward;

        RaycastHit shootHit;
        if (Physics.Raycast(shootRay, out shootHit, shootRange, layerShootable)) {
            Unit enemyUnit = shootHit.transform.parent?.parent?.GetComponent<Unit>();
            IKillable enemyKillable = enemyUnit?.GetComponent<IKillable>();

            if (enemyKillable != null)
                enemyKillable.TakeDamage(damage);

            gunLine.SetPosition(1, shootHit.point);
            ParticleSystem ins = Instantiate(hitParticles, shootHit.point, Quaternion.Euler(0, 0, 0));
            Destroy(ins, 2f);
        }
        else {
            gunLine.SetPosition(1, shootRay.origin + shootRay.direction * shootRange);
        }
    }

    private void DisableEffects() {
        gunLine.enabled = false;
        gunLight.enabled = false;
    }

    private void UpdateLaser() {
        gunLaser.SetPosition(0, bulletSpawnTransform.position);
        Ray shootRay = new Ray();
        shootRay.origin = bulletSpawnTransform.position;
        shootRay.direction = transform.forward;
        RaycastHit shootHit;
        if (Physics.Raycast(shootRay, out shootHit, shootRange, layerShootable)) {
            gunLaser.SetPosition(1, shootHit.point);
        }
        else {
            gunLaser.SetPosition(1, shootRay.origin + shootRay.direction * shootRange);
        }
    }

    
}
