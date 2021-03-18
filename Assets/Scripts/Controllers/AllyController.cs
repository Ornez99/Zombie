using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyController : IController {

    private StateMachine stateMachine;

    public StateMachine StateMachine { get => stateMachine; }
    public Unit Owner { get; private set; }

    public AllyController(Unit unit) {
        Owner = unit;
        stateMachine = StateMachineFactory.CreateStateMachine(unit, UnitType.Human);
    }

    public void SetStateMachine(StateMachine stateMachine) {
        this.stateMachine = stateMachine;
    }

    public void Tick() {
        stateMachine?.Tick();
    }

    public override string ToString() {
        return "AllyController";
    }

}
