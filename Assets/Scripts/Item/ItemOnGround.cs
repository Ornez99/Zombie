using UnityEngine;

public class ItemOnGround : MonoBehaviour, IInteractable
{
    [SerializeField]
    private GameObject highlight = null;

    public Item Item { get; set; }
    public bool Enabled => true;

    public void Highlight()
    {
        highlight.SetActive(true);
    }

    public void Interact(Unit unit)
    {
        Equipment equipment = unit.Equipment;
        EquipmentUI equipmentUI = unit.EquipmentUI;
        int freeSlot = equipment.GetFreeSlot();

        if (freeSlot != Equipment.NoFreeSlots)
        {
            equipment.AddItem(Item, freeSlot);
            equipmentUI.UpdateItemsUI(equipment);
            Destroy(gameObject);
        }
    }

    public void StopHighlight()
    {
        if (this == null)
            return;
        highlight.SetActive(false);
    }
}
