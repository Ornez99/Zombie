using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "ScriptableObjects/New Item", order = 1)]
public class Item : ScriptableObject
{
    [SerializeField]
    private new string name = default;

    [SerializeField]
    private Sprite sprite = default;

    [SerializeField]
    private OnItemUse[] onItemUse;

    [SerializeField]
    private OnItemEquip[] onItemEquip;

    [SerializeField]
    private BodyPart bodyPart;

    public string Name { get => name; }

    public Sprite Sprite { get => sprite; }

    public bool Useable { get => onItemUse != null && onItemUse.Length > 0; }

    public bool Equipable { get => onItemEquip != null && onItemEquip.Length > 0; }

    public BodyPart BodyPart { get => bodyPart; }

    public void Use(Unit unit, ItemInEquipment itemInEquipment)
    {
        for (int i = 0; i < onItemUse.Length; i++)
        {
            onItemUse[i].OnUse(unit);
        }
        unit.Equipment.RemoveItem(itemInEquipment);
        unit.EquipmentUI.UpdateItemsUI(unit.Equipment);
    }

    public void Equip(Unit unit, ItemInEquipment itemInEquipment)
    {
        for (int i = 0; i < onItemEquip.Length; i++)
        {
            onItemEquip[i].OnEquip(unit, itemInEquipment);
        }
    }

    

}
