using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Zombie : Unit {

    [SerializeField]
    private Transform visionRaysStartTransform;

    private void Update() {
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);

        if (currentHealth <= 0)
            return;
        if (damagedTimer > 0) {
            damagedTimer -= Time.deltaTime;
            if (damagedTimer <= 0)
                animator.SetBool("Damaged", false);
            return;
        }

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
