using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Unit {

    [SerializeField]
    private int smellValue = 0;

    private void Awake() {
        Vision = new Vision(transform, 12f);
    }

    private void Update() {
        Vision?.Tick();


        Controller.Tick();
        if (Drive.Node.SmellValue < smellValue)
            Drive.Node.SmellValue = smellValue;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(Drive.Node.CenterPos, Vector3.one);
    }

}
