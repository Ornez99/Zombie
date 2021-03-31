using UnityEngine;

public class Firearm : Weapon {

    [SerializeField]
    private Transform bulletSpawnTransform = null;
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
    private bool effectsEnabled = default;

    private void Update() {
        if (timeToNextShot > 0)
            timeToNextShot -= Time.deltaTime;

        if(effectsEnabled && timeToNextShot <= timeBetweenShots - timeBetweenShots * effectsDisplayTime)
            DisableEffects();

        UpdateLaser();
    }

    public override void Attack() {
        if (timeToNextShot <= 0)
            Shoot();
    }

    public override void AttackUnit(IKillable target) {
        Debug.Log($"AttackUnit is not implemented in {ToString()}.");
    }

    private void Shoot() {
        timeToNextShot = timeBetweenShots;
        gunAudio.Play();
        gunLight.enabled = true;

        gunLine.enabled = true;
        gunLine.SetPosition(0, bulletSpawnTransform.position);

        Ray shootRay = new Ray();
        shootRay.origin = bulletSpawnTransform.position;
        shootRay.direction = transform.forward;

        RaycastHit shootHit;
        if (Physics.Raycast(shootRay, out shootHit, attackRange, layerShootable)) {
            IKillable enemyKillable = shootHit.transform.GetComponent<IKillable>();

            if (enemyKillable != null)
                enemyKillable.TakeDamage(damage);

            gunLine.SetPosition(1, shootHit.point);
            GameObject ins = Instantiate(hitParticles, shootHit.point, Quaternion.Euler(0, 0, 0)).gameObject;
            Destroy(ins, 1f);
        }
        else {
            gunLine.SetPosition(1, shootRay.origin + shootRay.direction * attackRange);
        }

        effectsEnabled = true;
    }

    private void DisableEffects() {
        gunLine.enabled = false;
        gunLight.enabled = false;
        effectsEnabled = false;
    }

    private void UpdateLaser() {
        gunLaser.SetPosition(0, bulletSpawnTransform.position);

        Ray shootRay = new Ray();
        shootRay.origin = bulletSpawnTransform.position;
        shootRay.direction = transform.forward;

        RaycastHit shootHit;
        if (Physics.Raycast(shootRay, out shootHit, attackRange, layerShootable)) 
            gunLaser.SetPosition(1, shootHit.point);
        else 
            gunLaser.SetPosition(1, shootRay.origin + shootRay.direction * attackRange);
    }
}
