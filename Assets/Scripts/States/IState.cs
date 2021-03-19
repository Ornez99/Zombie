using UnityEngine;

public interface IState {

    int GetScore();
    void OnStateSelected();
    void OnStateDeselected();
    void Tick();

}
