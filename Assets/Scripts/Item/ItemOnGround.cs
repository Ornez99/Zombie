using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOnGround : MonoBehaviour, IInteractable {

    [SerializeField]
    private GameObject highlight = null;

    public Item Item { get; set; }
    public bool Enabled => true;

    public void Highlight() {
        highlight.SetActive(true);
    }

    public void Interact(Unit unit) {
        Equipment equipment = unit.Equipment;
        int freeSlot = equipment.GetFreeItemSlot();

        if (freeSlot != Equipment.NoFreeItemSlots) {
            equipment.AddItem(Item, freeSlot);
            equipment.UpdateUI();
            Destroy(gameObject);
        }
    }

    public void StopHighlight() {
        if (this == null)
            return;
        highlight.SetActive(false);
    }
}
