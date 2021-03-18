using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFactory : MonoBehaviour {

    public static WeaponFactory Instance;

    private Dictionary<WeaponType, GameObject> weaponPrefabs;

    public void Initialize() {
        if (Instance != null && Instance != this) {
            Debug.LogError("There can be only one instance of this script!");
            Destroy(this);
        }

        Instance = this;
        LoadWeaponsFromResources();
    }

    public Weapon SpawnWeapon(Transform parent, WeaponType weaponType) {
        Weapon weapon = Instantiate(weaponPrefabs[weaponType]).GetComponent<Weapon>();
        weapon.transform.position = parent.position;
        Vector3 eulerRotation = new Vector3(parent.eulerAngles.x, parent.eulerAngles.y, parent.eulerAngles.z);
        weapon.transform.rotation = Quaternion.Euler(eulerRotation);
        weapon.transform.SetParent(parent);
       
        return weapon;
    }

    private void LoadWeaponsFromResources() {
        weaponPrefabs = new Dictionary<WeaponType, GameObject>();
        GameObject[] weapons = Resources.LoadAll<GameObject>("Prefabs/Weapons");
        foreach (GameObject weapon in weapons) {
            WeaponType type = EnumMethods<WeaponType>.FromString(weapon.name);
            weaponPrefabs.Add(type, weapon);
        }
    }

}