using UnityEngine;

[CreateAssetMenu(fileName = "New ChangeSpeedOnUse", menuName = "ScriptableObjects/ItemsOnUse/ChangeSpeedOnUse", order = 1)]
public class ChangeSpeedOnUse : OnItemUse
{
    [SerializeField]
    private float speedModifier;
    [SerializeField]
    private float duration;

    public override void OnUse(Unit unit)
    {
        unit.Drive.SpeedModifier = speedModifier;
    }
}
