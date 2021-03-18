using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Zombie : Unit {

    private void Awake() {
        Vision = new Vision(transform, 8f);
    }

    private void Update() {
        
        Vision?.Tick();
        Controller.Tick();
    }
#if UNITY_EDITOR
    private void OnGUI()
    {
        Handles.Label(transform.position, Controller.StateMachine.ToString());
    }
#endif
    private void OnDrawGizmos()
    {
        if (Drive == null)
            return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(Drive.Node.CenterPos, Vector3.one);
    }
}
