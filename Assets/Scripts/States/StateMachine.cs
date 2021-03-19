using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine {

    private IState currentState;
    private List<IState> states;
    private Animator animator;

    public override string ToString() {
        return currentState.ToString();
    }

    public StateMachine(Unit unit) {
        states = new List<IState>();
        states.Add(new StateIdle(unit, unit.Animator));
        currentState = states[0];
        currentState.OnStateSelected();
        animator = unit.Animator;
    }

    public void Tick() {
        IState bestStateCandidate = states[0];
        int highestScore = states[0].GetScore();
        for (int i = 1; i < states.Count; i++) {
            if (highestScore < states[i].GetScore()) {
                highestScore = states[i].GetScore();
                bestStateCandidate = states[i];
            }
        }
        if (currentState != bestStateCandidate) {
            currentState.OnStateDeselected();
            currentState = bestStateCandidate;
            currentState.OnStateSelected();
        }
        currentState.Tick();
    }

    public void AddState(IState state) {
        states.Add(state);
    }
}
