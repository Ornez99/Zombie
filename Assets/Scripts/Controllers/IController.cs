using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IController {

    StateMachine StateMachine {get;}
    Unit Owner { get; }
    void Tick();
    string ToString();

}
