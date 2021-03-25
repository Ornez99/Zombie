using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControlledUnits : MonoBehaviour {

    private const int unitSlotButtonId = 0;
    private const int unitSlotHealthId = 1;
    private const int unitSlotSelectId = 2;

    [SerializeField]
    private Player player = null;

    [SerializeField]
    private Color32 colorUnitSelected;
    [SerializeField]
    private Color32 colorUnitNotSelected;
    [SerializeField]
    private Color32 colorHealthBar;

    [SerializeField]
    private GameObject unitSlotsParent = null;
    [SerializeField]
    private GameObject unitSlotPrefab = null;
    [SerializeField]
    private Dictionary<Unit, GameObject> unitSlots;

    public void Initialize() {
        colorUnitSelected = new Color32(0xCF, 0xB8, 0x4E, 0xFF);
        colorUnitNotSelected = new Color32(0xFF, 0xFF, 0xFF, 0xFF);
        colorHealthBar = new Color32(0x8D, 0x2B, 0x21, 0xFF);
        unitSlots = new Dictionary<Unit, GameObject>();
    }

    public void CreateNewUnitSlot(Unit unit) {
        GameObject unitSlot = Instantiate(unitSlotPrefab, unitSlotsParent.transform);
        unitSlot.transform.localPosition = new Vector3(unitSlots.Count * 128, 0, 0);
        unitSlots.Add(unit, unitSlot);
        Button button = unitSlot.transform.GetChild(unitSlotButtonId).GetComponent<Button>();
        button.onClick.AddListener(() => player.TakeControl(unit.GetComponent<Human>()));

        button.onClick.AddListener(() => TurnUnitSelectedSlot(unitSlot.transform.GetChild(unitSlotSelectId).GetComponent<Image>().color != colorUnitSelected, unit));
        unit.OnHealthChange += ChangeHealthBar;
        ChangeHealthBar(unit);
        unitSlots[unit].transform.GetChild(unitSlotButtonId).GetComponent<Image>().sprite = unit.FaceSprite;
    }

    public void TurnUnitSelectedSlot(bool value, Unit unit) {
        foreach (GameObject go in unitSlots.Values) {
            go.transform.GetChild(unitSlotSelectId).GetComponent<Image>().color = colorUnitNotSelected;
        }

        unitSlots[unit].transform.GetChild(unitSlotSelectId).GetComponent<Image>().color = (value) ? colorUnitSelected : colorUnitNotSelected;
    }

    private void ChangeHealthBar(Unit unit) {
        unitSlots[unit].transform.GetChild(unitSlotHealthId).transform.GetChild(1).GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (unit.CurrentHealth / unit.MaxHealth) * 128);
        unitSlots[unit].transform.GetChild(unitSlotHealthId).transform.GetChild(2).GetComponent<Text>().text = $"Health: {unit.CurrentHealth} / {unit.MaxHealth}";
    }

}
