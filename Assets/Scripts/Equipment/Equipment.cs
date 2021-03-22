using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Equipment : MonoBehaviour {

    public readonly static int NoFreeItemSlots = -1;
    private const int itemUIbackgroundId = 0;
    private const int itemUIbuttonId = 1;
    private const int itemUIisSelectedId = 2;

    [SerializeField]
    private int itemSlots;
    [SerializeField]
    private Item[] items;
    [SerializeField]
    private Transform equipmentUI;

    private void Awake() {
        items = new Item[itemSlots];
        equipmentUI = GameObject.Find("EquipmentUI").transform;
    }

    public int GetFreeItemSlot() {
        for (int i = 0; i < itemSlots; i++) {
            if (items[i] == null)
                return i;
        }

        return NoFreeItemSlots;
    }

    public void AddItem(Item item, int itemSlot) {
        items[itemSlot] = item;
    }

    public void UpdateUI() {
        for (int i = 0; i < itemSlots; i++) {
            if (items[i] != null) {
                equipmentUI.GetChild(i).GetChild(itemUIbuttonId).GetComponent<Button>().onClick.RemoveAllListeners();
                equipmentUI.GetChild(i).GetChild(itemUIbuttonId).GetComponent<Image>().sprite = items[i].ItemSprite;
                SetActiveSlot(i, true);
            }
            else {
                SetActiveSlot(i, false);
            }
        }
    }

    private void SetActiveSlot(int slotId, bool active) {
        equipmentUI.GetChild(slotId).GetChild(itemUIbuttonId).GetComponent<Image>().enabled = active;
        equipmentUI.GetChild(slotId).GetChild(itemUIbuttonId).GetComponent<Button>().enabled = active;
        equipmentUI.GetChild(slotId).GetChild(itemUIisSelectedId).GetComponent<Image>().enabled = active;
    }


}
