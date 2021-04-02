using UnityEngine;
using UnityEngine.UI;

public class EquipmentUIData : MonoBehaviour
{
    public Transform ItemSlotsParent;
    public Transform ItemOptionsMenu;

    public int ItemBackgroundId = 0;
    public int ItemButtonId = 1;
    public int ItemSelectedId = 2;

    public RectTransform ItemOptionsBackground;

    [Header("Use")]
    public Transform UseTranform;
    public RectTransform UseRectTransform;
    public Button UseButton;

    [Header("Equip")]
    public Transform EquipTranform;
    public RectTransform EquipRectTransform;
    public Button EquipButton;
    public Text EquipText;

    [Header("Drop")]
    public Transform DropTranform;
    public RectTransform DropRectTransform;
    public Button DropButton;

}
