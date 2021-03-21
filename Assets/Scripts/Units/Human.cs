using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Unit {

    [SerializeField]
    private int smellValue = 0;
    private RectTransform healthTransform;

    private new void Awake() {
        base.Awake();
    }

    private void FixedUpdate() {
        if (currentHealth <= 0)
            return;
        if (damagedTimer > 0)
        {
            damagedTimer -= Time.deltaTime * 5f;
            if (damagedTimer <= 0)
                animator.SetBool("Damaged", false);
            return;
        }

        Node = Map.GetNodeFromPos(transform.position);
        Controller.Tick();

        VisionInterpreter.Tick();

        Node = Map.GetNodeFromPos(transform.position);
        if (Node.SmellValue < smellValue)
            Node.SmellValue = smellValue;
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }

    private void Update() {
        
    }

    private void OnDrawGizmos() {
        if (Node != null)
            Gizmos.DrawWireCube(Node.CenterPos, Vector3.one);
    }
}
