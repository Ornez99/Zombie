using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Zombie : Unit {

    [SerializeField]
    private Transform visionRaysStartTransform;

    private void Update() {
        Node = Map.GetNodeFromPos(transform.position);
        Controller.Tick();
        VisionInterpreter.Tick();
        Node = Map.GetNodeFromPos(transform.position);
    }

    private void FixedUpdate() {
        Vision?.Tick();
    }


#if UNITY_EDITOR
    private void OnGUI() {
        Handles.Label(transform.position, Controller.StateMachine.ToString());
    }
#endif
    private void OnDrawGizmos() {
        if (Node == null)
            return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(Node.CenterPos, Vector3.one);
    }
}
