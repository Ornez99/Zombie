﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerController : IController {

    public static PlayerController Instance;

    private Transform unitTransform;
    private int groundMask = 1 << 8;
    private IInteractable currentInteractable;

    public StateMachine StateMachine { get => null; }
    public Unit Owner { get; private set; }

    public PlayerController(Unit unit) {
        Instance = this;
        Owner = unit;
        unitTransform = unit.transform;
    }

    public void Tick() {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if (Input.GetMouseButton(0))
            Owner.Weapon.Shoot();

        currentInteractable?.StopHighlight();
        currentInteractable = Owner.VisionInterpreter.ClosestInteractable;
        currentInteractable?.Highlight();

        if (Input.GetKeyDown(KeyCode.E))
            currentInteractable?.Interact();

        Vector3 normalVector3 = new Vector3(x, 0, z).normalized;
        Owner.Drive.MoveWithNormalizedDirection(normalVector3);
        Rotate();
    }

    public override string ToString() {
        return "PlayerController";
    }

    private void Rotate() {
        Vector3 mousePos = Input.mousePosition;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out hit, 100, groundMask)) {
            unitTransform.LookAt(new Vector3(hit.point.x, 0, hit.point.z));
        }
    }
}
