using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public static Player Instance;

    public GameObject ItemOnGround;

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

    public void TakeControl(Human unit) {
        if (playerController?.Owner != null) {
            Human currentControlledUnit = playerController.Owner.GetComponent<Human>();
            currentControlledUnit.Controller = new AllyController(currentControlledUnit);
            currentControlledUnit.FieldOfView = new FieldOfViewAlly(currentControlledUnit, 10f, 90f, currentControlledUnit.transform.GetChild(1));
            currentControlledUnit.Animator.SetBool("Run", false);
            currentControlledUnit.Animator.SetBool("Walk", false);
            currentControlledUnit.Animator.SetBool("RangedAttack", false);
        }

        cameraFollow.SetCameraTarget(unit.transform);
        playerController = new PlayerController(unit);
        unit.OnTakeControl(playerController);

        foreach (Human ownedUnit in ownedUnits) {
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

    public void RemoveOwnedHuman(Unit unit) {
        if (ownedUnits.Contains(unit)) {
            ownedUnits.Remove(unit);
        }
    }

    public int GetOwnedHumansCount() {
        return ownedUnits.Count;
    }

    public void CreateItemOnGround(Vector3 pos, Item item) {

        GameObject ins = Instantiate(ItemOnGround, pos, Quaternion.Euler(0, 0, 0));
        ItemOnGround itemOnGround = ins.GetComponent<ItemOnGround>();
        itemOnGround.Item = item;
    }
}
