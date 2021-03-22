using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Unit {

    [SerializeField]
    private int smellValue = 0;
    private RectTransform healthTransform;

    private void FixedUpdate() {
        if (isDead)
            return;

        Controller.Tick();

        VisionInterpreter.Tick();

        Node = Map.GetNodeFromPos(transform.position);
        if (Node.SmellValue < smellValue)
            Node.SmellValue = smellValue;
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }

    public void OnTakeControl(PlayerController playerController) {
        Controller = playerController;
        Animator.SetBool("Run", false);
        Animator.SetBool("Walk", false);
        Animator.SetBool("RangedAttack", false);
        Equipment.UpdateUI();
    }


}
