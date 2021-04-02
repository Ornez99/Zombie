using UnityEngine;
using UnityEngine.UI;

public class EquipmentUI : MonoBehaviour
{
    private EquipmentUIData UIData;

    private void Awake()
    {
        UIData = GameObject.Find("GameManager").GetComponent<EquipmentUIData>();
    }

    public void UpdateItemsUI(Equipment equipment)
    {
        for (int slotId = 0; slotId < equipment.ItemsSlots; slotId++)
        {
            ItemInEquipment item = equipment.GetItem(slotId);
            if (item != null)
            {
                UIData.ItemSlotsParent.GetChild(slotId).GetChild(UIData.ItemButtonId).GetComponent<Button>().onClick.RemoveAllListeners();
                UIData.ItemSlotsParent.GetChild(slotId).GetChild(UIData.ItemButtonId).GetComponent<Button>().onClick.AddListener(() => OpenItemOptionsMenu(item));
                UIData.ItemSlotsParent.GetChild(slotId).GetChild(UIData.ItemButtonId).GetComponent<Image>().sprite = item.ItemReference.Sprite;
                SetActiveSlot(slotId, true);
                UIData.ItemSlotsParent.GetChild(slotId).GetChild(UIData.ItemSelectedId).GetComponent<Image>().color = item.IsEquipped ? Color.yellow : Color.gray;
            }
            else
                SetActiveSlot(slotId, false);
        }
        CloseItemOptionsMenu();
    }

    public void OpenItemOptionsMenu(ItemInEquipment item)
    {
        UIData.ItemOptionsMenu.gameObject.SetActive(true);
        UIData.ItemOptionsMenu.transform.position = Input.mousePosition;

        float backgroundHeight = 0;
        float nextButtonPosY = -10;

        if (item.ItemReference.Useable)
        {
            UIData.UseTranform.gameObject.SetActive(true);
            UIData.UseButton.onClick.RemoveAllListeners();
            UIData.UseButton.onClick.AddListener(() => item.ItemReference.Use(item.ItemOwner, item));
            UIData.UseTranform.localPosition = new Vector3(0, nextButtonPosY, 0);

            nextButtonPosY -= UIData.UseRectTransform.sizeDelta.y;
            backgroundHeight += UIData.UseRectTransform.sizeDelta.y + 10;
        }
        else
            UIData.UseTranform.gameObject.SetActive(false);

        if (item.ItemReference.Equipable)
        {
            UIData.EquipTranform.gameObject.SetActive(true);
            UIData.EquipButton.onClick.RemoveAllListeners();
            UIData.EquipButton.onClick.AddListener(() => item.ItemReference.Equip(item.ItemOwner, item));
            UIData.EquipText.text = (item.IsEquipped) ? "Unequip" : "Equip";
            UIData.EquipTranform.localPosition = new Vector3(0, nextButtonPosY, 0);

            nextButtonPosY -= UIData.EquipRectTransform.sizeDelta.y;
            backgroundHeight += UIData.EquipRectTransform.sizeDelta.y + 10;
        }
        else
            UIData.EquipTranform.gameObject.SetActive(false);

        if (item.IsEquipped == false)
        {
            UIData.DropTranform.gameObject.SetActive(true);
            UIData.DropButton.onClick.RemoveAllListeners();
            UIData.DropButton.onClick.AddListener(() => item.Drop(item));
            UIData.DropTranform.localPosition = new Vector3(0, nextButtonPosY, 0);

            nextButtonPosY -= UIData.DropRectTransform.sizeDelta.y;
            backgroundHeight += UIData.DropRectTransform.sizeDelta.y + 10;
        }
        else
            UIData.DropTranform.gameObject.SetActive(false);

        UIData.ItemOptionsBackground.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, backgroundHeight);
    }

    private void SetActiveSlot(int slotId, bool active)
    {
        UIData.ItemSlotsParent.GetChild(slotId).GetChild(UIData.ItemButtonId).GetComponent<Image>().enabled = active;
        UIData.ItemSlotsParent.GetChild(slotId).GetChild(UIData.ItemButtonId).GetComponent<Button>().enabled = active;
        UIData.ItemSlotsParent.GetChild(slotId).GetChild(UIData.ItemSelectedId).GetComponent<Image>().enabled = active;
    }

    public void CloseItemOptionsMenu()
    {
        UIData.ItemOptionsMenu.gameObject.SetActive(false);
    }

}
