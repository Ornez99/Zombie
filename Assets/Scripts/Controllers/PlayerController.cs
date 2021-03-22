using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerController : IController {

    public static PlayerController Instance;

    private Transform unitTransform;
    private int groundMask = 1 << 8;
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
        if (Input.GetMouseButton(0))
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
        Vector3 normalVector3 = new Vector3(moveX, 0, moveZ).normalized;
        Owner.Drive.MoveWithNormalizedDirection(normalVector3);
        if (Mathf.Abs(moveX) > 0.2f || Mathf.Abs(moveZ) > 0.2f)
            Owner.Animator.SetBool("Run", true);
    }

    private void Rotate() {
        Vector3 mousePos = Input.mousePosition;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out hit, 100, groundMask)) {
            unitTransform.LookAt(new Vector3(hit.point.x, 0, hit.point.z));
            unitTransform.rotation = Quaternion.Euler(0, unitTransform.rotation.eulerAngles.y, 0);
        }
            
    }

    private void UpdateInteractionObject() {
        currentInteractable?.StopHighlight();
        currentInteractable = Owner.VisionInterpreter.ClosestInteractable;
        currentInteractable?.Highlight();
    }

    private void InteractWithObject() {
        currentInteractable?.Interact(Owner);
    }

    private void Attack() {
        Owner.Animator.SetBool("RangedAttack", true);
        Owner.Weapon.Attack();
    }
}
