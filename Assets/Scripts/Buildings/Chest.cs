using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Building, IInteractable {

    [SerializeField]
    private List<Item> itemsInside;
    private bool isEmpty;
    [SerializeField]
    private GameObject highlight = null;

    public bool Enabled { get => !isEmpty; }

    public void Interact(Unit unit) {
        Equipment equipment = unit.Equipment;
        for (int i = itemsInside.Count - 1; i >= 0; i--) {
            Item item = itemsInside[i];
            bool itemsAdded = false;
            int freeSlot = equipment.GetFreeItemSlot();

            if (freeSlot != Equipment.NoFreeItemSlots) {
                equipment.AddItem(item, freeSlot);
                itemsAdded = true;
            }
            else {
                if (itemsAdded)
                    equipment.UpdateUI();
                return;
            }

            if (itemsAdded)
                equipment.UpdateUI();
            itemsInside.RemoveAt(i);
        }

        isEmpty = true;
    }

    public void Highlight() {
        highlight.SetActive(true);
    }

    public void StopHighlight() {
        highlight.SetActive(false);
    }
}
