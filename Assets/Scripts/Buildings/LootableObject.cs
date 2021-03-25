using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootableObject : Building, IInteractable {

    [SerializeField]
    private LootChance[] lootChance = null;
    [SerializeField]
    private List<Item> itemsInside;
    [SerializeField]
    private GameObject highlight = null;
    private bool isEmpty;

    public bool Enabled { get => !isEmpty; }

    private void Awake() {
        itemsInside = new List<Item>();
        foreach (LootChance loot in lootChance) {
            for (int i = 0; i < loot.Amount; i++) {
                int randomValue = Random.Range(0, 100);
                if (loot.SpawnChance > randomValue) {
                    itemsInside.Add(loot.Item);
                }
            }
        }

        if (itemsInside.Count == 0)
            isEmpty = true;
    }

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

    [System.Serializable]
    public struct LootChance {
        public int Amount;
        public int SpawnChance;
        public Item Item;
    }

}
