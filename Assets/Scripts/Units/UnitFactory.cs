using System.Collections.Generic;
using UnityEngine;

public class UnitFactory : MonoBehaviour {

    public static UnitFactory Instance;

    private Dictionary<UnitType, GameObject> unitPrefabs;

    public void Initialize() {
        if (Instance != null && Instance != this) {
            Debug.LogError("There can be only one instance of this script!");
            Destroy(this);
        }

        Instance = this;
        LoadUnitsFromResources();
    }

    public Unit SpawnUnit(Vector3 position, Quaternion rotation, UnitType unitType) {
        Unit unit = Instantiate(unitPrefabs[unitType], position, rotation).GetComponent<Unit>();
        AddWeapon(unit, unitType);
        AddController(unit, unitType);
        return unit;
    }

    private void AddWeapon(Unit unit, UnitType unitType) {
        switch (unitType) {
            case UnitType.Human:
                unit.FieldOfView = new FieldOfViewPlayer(unit, 10f, 90f, unit.transform.GetChild(1));
                break;
            case UnitType.Human1:
                unit.FieldOfView = new FieldOfViewAlly(unit, 10f, 90f, unit.transform.GetChild(1));
                break;
            case UnitType.Human2:
                unit.FieldOfView = new FieldOfViewAlly(unit, 10f, 90f, unit.transform.GetChild(1));
                break;
            case UnitType.Zombie:
                unit.Weapon = WeaponFactory.Instance.SpawnWeapon(unit.Hand, WeaponType.Teeth);
                unit.FieldOfView = new FieldOfViewEnemy(unit, 8f, 90f, unit.transform.GetChild(1));
                break;
        }
    }

    private void AddController(Unit unit, UnitType unitType) {
        switch (unitType) {
            case UnitType.Human:
            case UnitType.Human1:
            case UnitType.Human2:
                unit.Controller = new AllyController(unit); 
                break;
            case UnitType.Zombie:
                unit.Controller = new EnemyController(unit);
                break;
        }
    }

    private void LoadUnitsFromResources() {
        unitPrefabs = new Dictionary<UnitType, GameObject>();
        GameObject[] units = Resources.LoadAll<GameObject>("Prefabs/Units");
        foreach (GameObject unit in units) {
            UnitType type = EnumMethods<UnitType>.FromString(unit.name);
            unitPrefabs.Add(type, unit);
        }
    }
}
