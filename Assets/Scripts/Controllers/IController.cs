using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IController {

    Unit Owner { get; }
    void Tick();
    string ToString();
}
