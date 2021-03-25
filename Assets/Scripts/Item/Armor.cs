using UnityEngine;

[CreateAssetMenu(fileName = "New Armor", menuName = "ScriptableObjects/Items/Armor", order = 1)]
public class Armor : Item {

    [SerializeField]
    private int armor = default;
    [SerializeField]
    private BodyPart bodyPart = default;
    [SerializeField]
    private bool isEquipped = default;

    public override bool IsEquipped { get => isEquipped; }
    public BodyPart BodyPart { get => bodyPart; }
    public int ArmorValue { get => armor; }

    public Armor(int armor, BodyPart bodyPart) {
        this.armor = armor;
        this.bodyPart = bodyPart;
    }

    public override void Equip(Unit unit) {
        IKillable killable = unit.GetComponent<IKillable>();
        Equipment equipment = unit.Equipment;
        if (isEquipped == true) {
            Unequip(killable);
            equipment.UpdateUI();
        }
        else if (CanEquip(equipment)) {
            isEquipped = true;
            killable.Armor += armor;
            equipment.UpdateUI();
        }
    }

    private void Unequip(IKillable killable) {
        killable.Armor -= armor;
        isEquipped = false;
    }

    private bool CanEquip(Equipment equipment) {
        for (int i = 0; i < equipment.Items.Length; i++) {
            Armor armorItem = equipment.Items[i] as Armor;
            if (armorItem != null)
                if (armorItem.BodyPart == bodyPart && armorItem.IsEquipped)
                    return false;
        }

        return true;
    }


}
