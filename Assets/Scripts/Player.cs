using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public static Player Instance;

    [SerializeField]
    private UIControlledUnits uIControlledUnits = null;
    [SerializeField]
    private CameraFollow cameraFollow = null;
    [SerializeField]
    private PlayerController playerController = null;
    [SerializeField]
    private List<Unit> ownedUnits;

    public void Initialize() {
        if (Instance != null && Instance != this) {
            Debug.LogError("There can be only one instance of this script!");
            Destroy(this);
        }

        ownedUnits = new List<Unit>();
        uIControlledUnits.Initialize();
        Instance = this;
    }

    private void Update() {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButtonUp(0)) {
            Vector3 mousePos = Input.mousePosition;
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            if (Physics.Raycast(ray, out hit, 100)) {
                if (hit.transform.GetComponent<Unit>() != null) {
                    //TakeControl(hit.transform.GetComponent<Unit>());
                }
            }
        }

        if (Input.GetKey(KeyCode.P)) {
            PlaceBarricade();
        }

    }

    public void TakeControl(Unit unit) {
        if (playerController?.Owner != null) {
            Unit currentControlledUnit = playerController.Owner.GetComponent<Unit>();
            currentControlledUnit.Controller = new AllyController(currentControlledUnit);
        }

        cameraFollow.SetCameraTarget(unit.transform);
        playerController = new PlayerController(unit);
        unit.OnTakeControl(playerController);
        
    }

    public void AddOwnedHuman(Unit unit) {
        if (!ownedUnits.Contains(unit)) {
            ownedUnits.Add(unit);
            uIControlledUnits.CreateNewUnitSlot(unit);
        }
    }

    private void PlaceBarricade() {
        Transform ownerTransform = playerController.Owner.transform;
        Node nodeToPlace = Map.GetNodeFromPos(ownerTransform.forward * 2f + ownerTransform.position);
        if (nodeToPlace.Buildable == true) {
            BuildingFactory.Instance.SpawnBuilding(nodeToPlace.CenterPos, Quaternion.identity, BuildingType.Barricade);
            nodeToPlace.Buildable = false;
            nodeToPlace.Walkable = false;
        }
        
    }
}
