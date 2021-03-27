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

    public GameObject TestFOW;

    public void Initialize() {
        Texture2D testTexture2D = new Texture2D(Map.Instance.MapSize, Map.Instance.MapSize);
        for (int y = 0; y < Map.Instance.MapSize; y++) {
            for (int x = 0; x < Map.Instance.MapSize; x++) {
                Color32 color1 = new Color32(0, 0, 0, 255);
                Color32 color2 = new Color32(0, 0, 0, 127);
                Color32 col = Random.Range(0, 2) == 1 ? color1 : color2;
                testTexture2D.SetPixel(x,y, col);
            }
        }


        testTexture2D.Apply();
        TestFOW.GetComponent<Renderer>().material.SetTexture("_MainTex", testTexture2D);







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

    public void TakeControl(Human unit) {
        if (playerController?.Owner != null) {
            Human currentControlledUnit = playerController.Owner.GetComponent<Human>();
            currentControlledUnit.Controller = new AllyController(currentControlledUnit);
            currentControlledUnit.Animator.SetBool("Run", false);
            currentControlledUnit.Animator.SetBool("Walk", false);
            currentControlledUnit.Animator.SetBool("RangedAttack", false);
        }

        cameraFollow.SetCameraTarget(unit.transform);
        playerController = new PlayerController(unit);
        unit.OnTakeControl(playerController);


        foreach(Human ownedUnit in ownedUnits) {
            if (ownedUnit == null)
                continue;

            if (unit == ownedUnit)
                continue;

            StateFollowTarget stateFollowTarget = (StateFollowTarget)ownedUnit.Controller.StateMachine.GetState("StateFollowTarget");
            stateFollowTarget?.SetTarget(unit);
        }
    }

    public void AddOwnedHuman(Unit unit) {
        if (!ownedUnits.Contains(unit)) {
            ownedUnits.Add(unit);
            uIControlledUnits.CreateNewUnitSlot(unit);
        }

        if (ownedUnits.Count == 1)
            uIControlledUnits.TurnUnitSelectedSlot(true, unit);
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
