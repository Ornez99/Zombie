using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Unit {

    private void Update() {
        Controller.Tick();
    }

}
