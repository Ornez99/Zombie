using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : IController {

    private StateMachine stateMachine;

    public Unit Owner { get; private set; }

    public StateMachine StateMachine { get => stateMachine; }

    public EnemyController(Unit unit) {
        Owner = unit;
        stateMachine = StateMachineFactory.CreateStateMachine(unit, UnitType.Zombie);
    }

    public void Tick() {
        stateMachine.Tick();
    }

    public override string ToString() {
        return "EnemyController";
    }
}
