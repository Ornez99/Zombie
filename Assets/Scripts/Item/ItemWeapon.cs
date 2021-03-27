using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "ScriptableObjects/Items/Weapon", order = 1)]
public class ItemWeapon : Item {

    [SerializeField]
    private GameObject weaponPrefab = default;
    [SerializeField]
    private bool isEquipped = default;
    [SerializeField]
    private WeaponType weaponType = default;

    public override bool IsEquipped { get => isEquipped; }
    public WeaponType WeaponType { get => weaponType; }
    public GameObject WeaponPrefab { get => weaponPrefab; }

    public ItemWeapon(GameObject weaponPrefab, WeaponType weaponType) {
        this.weaponPrefab = weaponPrefab;
        this.weaponType = weaponType;
    }

    public override void Equip(Unit unit) {
        Equipment equipment = unit.Equipment;
        if (isEquipped == true) {
            Unequip(unit);
            equipment.UpdateUI();
        }
        else if (CanEquip(equipment)) {
            isEquipped = true;
            unit.Weapon = WeaponFactory.Instance.SpawnWeapon(unit.Hand, weaponType);
            equipment.UpdateUI();
        }
    }

    private void Unequip(Unit unit) {
        isEquipped = false;
        Destroy(unit.Weapon.gameObject);
    }

    private bool CanEquip(Equipment equipment) {
        for (int i = 0; i < equipment.Items.Length; i++) {
            ItemWeapon itemWeapon = equipment.Items[i] as ItemWeapon;
            if (itemWeapon != null)
                if (itemWeapon.IsEquipped)
                    return false;
        }
        return true;
    }

}
