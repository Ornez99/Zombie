using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyController : IController {

    public StateMachine StateMachine { get; private set; }
    public Unit Owner { get; private set; }

    public AllyController(Unit unit) {
        Owner = unit;
        StateMachine = StateMachineFactory.CreateStateMachine(unit, UnitType.Human);
    }

    public void SetStateMachine(StateMachine stateMachine) {
        StateMachine = stateMachine;
    }

    public void Tick() {
        StateMachine?.Tick();
    }

    public override string ToString() {
        return "AllyController";
    }

}
