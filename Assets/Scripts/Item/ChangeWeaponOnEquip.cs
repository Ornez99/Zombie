using UnityEngine;

[CreateAssetMenu(fileName = "New ChangeWeaponOnEquip", menuName = "ScriptableObjects/ItemsOnEquip/ChangeWeaponOnEquip", order = 1)]
public class ChangeWeaponOnEquip : OnItemEquip
{
    [SerializeField]
    private WeaponType weaponType;

    public override void OnEquip(Unit unit, ItemInEquipment itemInEquipment)
    {
        if (itemInEquipment.IsEquipped == false && CanEquipWeapon(unit.Equipment) == true)
        {
            unit.Weapon = WeaponFactory.Instance.SpawnWeapon(unit.Hand, weaponType);
            itemInEquipment.IsEquipped = true;
        }
        else if (itemInEquipment.IsEquipped == true)
        {
            Destroy(unit.Weapon.gameObject);
            itemInEquipment.IsEquipped = false;
        }

        unit.EquipmentUI.UpdateItemsUI(unit.Equipment);
    }

    private bool CanEquipWeapon(Equipment equipment)
    {
        for (int i = 0; i < equipment.ItemsSlots; i++)
        {
            if (equipment.GetItem(i)?.IsEquipped == true && equipment.GetItem(i)?.ItemReference.BodyPart == BodyPart.Hand)
            {
                return false;
            }
        }

        return true;
    }

}
