using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Unit {

    [SerializeField]
    private int smellValue = 0;

    private void Update() {
        Node = Map.GetNodeFromPos(transform.position);
        Controller.Tick();

        VisionInterpreter.Tick();

        Node = Map.GetNodeFromPos(transform.position);
        if (Node.SmellValue < smellValue)
            Node.SmellValue = smellValue;
    }

    private void FixedUpdate() {
        //Vision?.Tick();
        //VisionInterpreter?.Tick();
    }

    private void OnDrawGizmos() {
        if (Node != null)
            Gizmos.DrawWireCube(Node.CenterPos, Vector3.one);
    }

}
