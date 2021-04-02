using UnityEngine;

[CreateAssetMenu(fileName = "New HealOnUse", menuName = "ScriptableObjects/ItemsOnUse/HealOnUse", order = 1)]
public class HealOnUse : OnItemUse
{
    [SerializeField]
    private float healAmount;

    public override void OnUse(Unit unit)
    {
        IKillable killable = unit as IKillable;
        if (killable != null)
        {
            killable.Heal(healAmount);
        }
    }
}
