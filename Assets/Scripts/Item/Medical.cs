using UnityEngine;

[CreateAssetMenu(fileName = "New Medical", menuName = "ScriptableObjects/Items/Medical", order = 1)]
public class Medical : Item {

    [SerializeField]
    private float healAmount;

    public float HealAmount { get => healAmount; }

    public Medical(float healAmount) {
        this.healAmount = healAmount;
    }

    public override void Use(Unit unit) {
        IKillable killable = unit.GetComponent<IKillable>();
        killable.Heal(healAmount);
        unit.Equipment.RemoveItem(this);

    }
}
