using UnityEngine;

[CreateAssetMenu(fileName = "New UnitData", menuName = "ScriptableObjects/UnitData", order = 1)]
public class UnitData : ScriptableObject
{
    public int Team;
    public float MaxHealth;

    public float MovementSpeedRun;
    public float MovementSpeedWalk;
}