using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : IController {

    public static PlayerController Instance;

    private Transform unitTransform;
    private int groundMask2 = 1 << 10;
    private IInteractable currentInteractable;

    private float moveX;
    private float moveZ;

    public StateMachine StateMachine { get => null; }
    public Unit Owner { get; private set; }

    public PlayerController(Unit unit) {
        Instance = this;
        Owner = unit;
        unitTransform = unit.transform;
    }

    public void Tick() {
        UpdateMovementInput();
        Owner.Animator.SetBool("Run", false);
        Move();
        Rotate();
        UpdateInteractionObject();

        if (Input.GetKeyDown(KeyCode.E))
            InteractWithObject();

        Owner.Animator.SetBool("RangedAttack", false);
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
            Attack();
    }

    public override string ToString() {
        return "PlayerController";
    }

    private void UpdateMovementInput() {
        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");
    }

    private void Move() {
        //Vector3 normalVector3 = (Owner.transform.forward * moveZ + Owner.transform.right * moveX).normalized;

        Vector3 normalVector3 = new Vector3(moveX, 0, moveZ).normalized;
        normalVector3 = Quaternion.Euler(0, 45, 0) * normalVector3;
        Owner.Drive.MoveWithNormalizedDirection(normalVector3);
        if (Mathf.Abs(moveX) > 0.2f || Mathf.Abs(moveZ) > 0.2f)
            Owner.Animator.SetBool("Run", true);
    }

    private void Rotate() {
        Vector3 mousePos = Input.mousePosition;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out hit, 100, groundMask2)) {
            unitTransform.LookAt(new Vector3(hit.point.x, hit.point.y, hit.point.z));
            unitTransform.rotation = Quaternion.Euler(0, unitTransform.rotation.eulerAngles.y, 0);
        }
            
    }

    private void UpdateInteractionObject() {
        currentInteractable?.StopHighlight();
        currentInteractable = Owner.FieldOfView.ClosestInteractable;
        currentInteractable?.Highlight();
    }

    private void InteractWithObject() {
        currentInteractable?.Interact(Owner);
    }

    private void Attack() {
        Owner.Animator.SetBool("RangedAttack", true);
        Owner.Weapon?.Attack();
    }
}
