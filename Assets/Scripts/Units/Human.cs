using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Unit {

    [SerializeField]
    private int smellValue = 0;

    private void Update() {
        Controller.Tick();
        if (Drive.Node.SmellValue < smellValue)
            Drive.Node.SmellValue = smellValue;
    }

    

}
