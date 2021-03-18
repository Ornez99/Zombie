using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine {

    private IState currentState;
    private List<IState> states;

    public override string ToString() {
        return currentState.ToString();
    }

    public StateMachine(Unit unit) {
        states = new List<IState>();
        states.Add(new StateIdle(unit));
        currentState = states[0];
    }

    public void Tick() {
        foreach (IState state in states) {
            if (currentState.GetScore() < state.GetScore()) {
                currentState = state;
            }
        }
        currentState.Tick();
    }

    public void AddState(IState state) {
        states.Add(state);
    }

}
