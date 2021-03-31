using UnityEngine;

public class Teeth : Weapon {

    private void Update() {
        if (timeToNextShot > 0)
            timeToNextShot -= Time.deltaTime;
    }

    public override void Attack() {
        Debug.Log($"Attack is not implemented in {ToString()}.");
    }

    public override void AttackUnit(IKillable target) {
        if (timeToNextShot <= 0) {
            target.TakeDamage(damage);
            timeToNextShot = timeBetweenShots;
        }
    }
}