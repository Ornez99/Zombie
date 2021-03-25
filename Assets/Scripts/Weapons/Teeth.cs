using UnityEngine;

public class Teeth : Weapon {

    private Unit unit;
    [SerializeField]
    private Transform biteTransform = null;
    [SerializeField]
    private float biteRadius = 1f;
    [SerializeField]
    private float biteDamage = 1f;
    [SerializeField]
    private float timeBetweenShoots = 1f;

    public Unit Unit { get => unit; set => unit = value; }

    private void Update() {
        if (timer > 0)
            timer -= Time.deltaTime;
    }

    public override void Attack() {
        if (timer <= 0) {
            RaycastHit[] hits = Physics.SphereCastAll(biteTransform.position, biteRadius, biteTransform.forward);
            foreach (RaycastHit hit in hits) {
                Unit hitUnit = hit.transform.GetComponent<Unit>();
                IKillable hitKillable = hitUnit?.GetComponent<IKillable>();
                if (hitKillable == null)
                    continue;

                if (hitUnit.GetTeam != unit.GetTeam) {
                    hitKillable.TakeDamage(biteDamage);
                    timer = timeBetweenShoots;
                }
            }
        }
    }

    public override void AttackUnit(IKillable target) {
        if (timer <= 0) {
            target.TakeDamage(biteDamage);
            timer = timeBetweenShoots;
        }
    }
}
