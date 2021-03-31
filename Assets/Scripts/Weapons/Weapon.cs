using UnityEngine;

public abstract class Weapon : MonoBehaviour {

    [SerializeField]
    protected float damage = 5f;
    [SerializeField]
    protected float attackRange = 5f;
    [SerializeField]
    protected float timeBetweenShots = 1f;
    protected float timeToNextShot = 0f;
    [SerializeField]
    protected float effectsDisplayTime = 0.2f;
    
    public float TimeBetweenShots { get => timeBetweenShots; }
    public float AttackRange { get => attackRange; }

    public abstract void Attack();
    public abstract void AttackUnit(IKillable target);
}