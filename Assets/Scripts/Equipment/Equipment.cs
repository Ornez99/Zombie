using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Equipment : MonoBehaviour {

    public readonly static int NoFreeItemSlots = -1;

    private const int itemUIbackgroundId = 0;
    private const int itemUIbuttonId = 1;
    private const int itemUIisSelectedId = 2;

    private const int itemOptionsMenuBackgroundId = 0;
    private const int itemOptionsMenuUseId = 1;
    private const int itemOptionsMenuEquipId = 2;

    [SerializeField]
    private int itemSlots = 0;
    [SerializeField]
    private Item[] items;
    [SerializeField]
    private Transform equipmentUI;
    [SerializeField]
    private GameObject itemOptionsMenu;

    public Item[] Items { get => items; }

    private void Awake() {
        if (items?.Length == 0 || items == null)
            items = new Item[itemSlots];
        equipmentUI = GameObject.Find("EquipmentUI").transform;
        itemOptionsMenu = UIControlledUnits.Instance.ItemOptionsMenu;
    }

    public int GetFreeItemSlot() {
        for (int i = 0; i < itemSlots; i++) {
            if (items[i] == null)
                return i;
        }

        return NoFreeItemSlots;
    }

    public void AddItem(Item item, int itemSlot) {
        Armor itemArmor = item as Armor;
        if (itemArmor != null) {
            items[itemSlot] = new Armor(itemArmor.ArmorValue, itemArmor.BodyPart);
            LoadDefaultItemData(items[itemSlot], item);
            return;
        }

        Medical itemMedical = item as Medical;
        if (itemMedical != null) {
            items[itemSlot] = new Medical(itemMedical.HealAmount);
            LoadDefaultItemData(items[itemSlot], item);
            return;
        }

        ItemWeapon itemWeapon = item as ItemWeapon;
        if (itemWeapon != null) {
            items[itemSlot] = new ItemWeapon(itemWeapon.WeaponPrefab, itemWeapon.WeaponType);
            LoadDefaultItemData(items[itemSlot], item);
            return;
        }

    }

    private void LoadDefaultItemData(Item newItem, Item dataItem) {
        newItem.ItemName = dataItem.ItemName;
        newItem.ItemSprite = dataItem.ItemSprite;
        newItem.Useable = dataItem.Useable;
        newItem.Equipable = dataItem.Equipable;
    }


    public void RemoveItem(Item item) {
        for (int i = 0; i < items.Length; i++) {
            if (items[i] == item) {
                Destroy(items[i]);
                items[i] = null;
                UpdateUI();
                return;
            }
        }
    }

    public void UpdateUI() {
        CloseItemOptionsMenu();
        for (int i = 0; i < itemSlots; i++) {
            if (items[i] != null) {
                Item currentItem = items[i];
                equipmentUI.GetChild(i).GetChild(itemUIbuttonId).GetComponent<Button>().onClick.RemoveAllListeners();
                equipmentUI.GetChild(i).GetChild(itemUIbuttonId).GetComponent<Button>().onClick.AddListener(() => OpenItemOptionsMenu(currentItem));
                equipmentUI.GetChild(i).GetChild(itemUIbuttonId).GetComponent<Image>().sprite = items[i].ItemSprite;
                SetActiveSlot(i, true);
                ShowEquippedItem(i);

            }
            else {
                SetActiveSlot(i, false);
            }
        }
    }

    public void CloseItemOptionsMenu() {
        itemOptionsMenu.SetActive(false);
    }

    private void OpenItemOptionsMenu(Item item) {
        itemOptionsMenu.SetActive(true);
        itemOptionsMenu.transform.position = Input.mousePosition;
        float backgroundHeight = 0;
        float nextButtonPosY = -10;

        if (item.Useable) {
            Transform buttonUseTransform = itemOptionsMenu.transform.GetChild(itemOptionsMenuUseId);
            Button buttonUse = buttonUseTransform.GetComponent<Button>();

            buttonUseTransform.gameObject.SetActive(true);
            buttonUse.onClick.RemoveAllListeners();
            buttonUse.onClick.AddListener(() => item.Use(GetComponent<Unit>()));
            buttonUseTransform.transform.localPosition = new Vector3(0, nextButtonPosY, 0);

            nextButtonPosY -= 40;
            backgroundHeight += 50;
        }
        else
            itemOptionsMenu.transform.GetChild(itemOptionsMenuUseId).gameObject.SetActive(false);

        if (item.Equipable) {
            Transform buttonEquipTransform = itemOptionsMenu.transform.GetChild(itemOptionsMenuEquipId);
            Button buttonEquip = buttonEquipTransform.GetComponent<Button>();
            buttonEquipTransform.gameObject.SetActive(true);

            buttonEquip.onClick.RemoveAllListeners();
            buttonEquip.onClick.AddListener(() => item.Equip(GetComponent<Unit>()));
            buttonEquip.transform.GetChild(0).GetComponent<Text>().text = (item.IsEquipped) ? "Unequip" : "Equip";
            buttonEquipTransform.transform.localPosition = new Vector3(0, nextButtonPosY, 0);

            nextButtonPosY -= 40;
            backgroundHeight += 50;
        }
        else
            itemOptionsMenu.transform.GetChild(itemOptionsMenuEquipId).gameObject.SetActive(false);

        itemOptionsMenu.transform.GetChild(itemOptionsMenuBackgroundId).GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, backgroundHeight);
    }

    private void SetActiveSlot(int slotId, bool active) {
        equipmentUI.GetChild(slotId).GetChild(itemUIbuttonId).GetComponent<Image>().enabled = active;
        equipmentUI.GetChild(slotId).GetChild(itemUIbuttonId).GetComponent<Button>().enabled = active;
        equipmentUI.GetChild(slotId).GetChild(itemUIisSelectedId).GetComponent<Image>().enabled = active;
    }

    private void ShowEquippedItem(int slotId) {
        if (items[slotId].IsEquipped)
            equipmentUI.GetChild(slotId).GetChild(itemUIisSelectedId).GetComponent<Image>().color = Color.yellow;
        else
            equipmentUI.GetChild(slotId).GetChild(itemUIisSelectedId).GetComponent<Image>().color = Color.gray;
    }


}
