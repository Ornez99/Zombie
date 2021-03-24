using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/Weapon", order = 1)]
public class ItemWeapon : Item, IEquippable {

    private bool isEquipped;

    public void Equip(Unit unit) {
        
    }

    public void Unequip(Unit unit) {
        
    }
}
