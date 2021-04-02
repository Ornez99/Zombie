using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{

    public static int NoFreeSlots = -1;

    [SerializeField]
    private Unit unit;

    [SerializeField]
    private int itemsSlots = 8;

    [SerializeField]
    private ItemInEquipment[] items;

    [SerializeField]
    private EquipmentUI equipmentUI;

    public int ItemsSlots { get => itemsSlots; }

    private void Awake()
    {
        items = new ItemInEquipment[itemsSlots];
    }

    public void AddItem(Item item, int slot)
    {
        ItemInEquipment newItem = new ItemInEquipment(unit, item);
        items[slot] = newItem;
    }

    public void RemoveItem(ItemInEquipment item)
    {
        for(int i = 0; i < items.Length; i++)
        {
            if(items[i] == item)
            {
                items[i] = null;
                break;
            }
        }
    }

    public int GetFreeSlot()
    {
        for(int i = 0; i < itemsSlots; i++)
        {
            if (GetItem(i) == null)
                return i;
        }

        return NoFreeSlots;
    }

    public ItemInEquipment GetItem(int slotId)
    {
        if (slotId >= items.Length)
            return null;

        return items[slotId];
    }
}
