using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField]
    private Pathfinding pathfinding = null;
    [SerializeField]
    private SmellManager smellManager = null;
    [SerializeField]
    private UnitFactory unitFactory = null;
    [SerializeField]
    private BuildingFactory buildingFactory = null;
    [SerializeField]
    private WeaponFactory weaponFactory = null;
    [SerializeField]
    private Map map = null;
    [SerializeField]
    private DayNightSystem dayNightSystem = null;
    [SerializeField]
    private Player player = null;
    [SerializeField]
    private Transform resolutionTransform = null;

    private void Awake() {
        Initialize();
        SetResolution();
        Spawn3Humans();
    }

    private void Initialize() {
        weaponFactory.Initialize();
        unitFactory.Initialize();
        buildingFactory.Initialize();
        map.Initialize();
        pathfinding.Initialize();
        smellManager.Initialize();
        player.Initialize();
        dayNightSystem.Initialize();
    }

    private void SetResolution() {
        resolutionTransform.localScale = new Vector3(Screen.width / 1920.0f, Screen.height / 1080.0f, 1);
    }

    private void Spawn3Humans() {
        Unit unit1 = unitFactory.SpawnUnit(new Vector3(32.5f, 0, 32.5f), Quaternion.Euler(0, 0, 0), UnitType.Human);
        Unit unit2 = unitFactory.SpawnUnit(new Vector3(32.5f, 0, 33.5f), Quaternion.Euler(0, 0, 0), UnitType.Human1);
        Unit unit3 = unitFactory.SpawnUnit(new Vector3(33.5f, 0, 33.5f), Quaternion.Euler(0, 0, 0), UnitType.Human2);
        player.AddOwnedHuman(unit1);
        player.AddOwnedHuman(unit2);
        player.AddOwnedHuman(unit3);

        player.TakeControl(unit1);
    }

}
