using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyController : IController {

    public Unit Owner { get; private set; }

    public AllyController(Unit unit) {
        Owner = unit;
    }

    public void Tick() {
        if (Vector3.Distance(PlayerController.Instance.Owner.transform.position, Owner.transform.position) > 5f) {
            
        }
    }

    public override string ToString() {
        return "AllyController";
    }

}
