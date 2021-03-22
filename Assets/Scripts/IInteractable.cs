using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable {

    bool Enabled { get; }
    void Interact(Unit unit);
    void Highlight();
    void StopHighlight();

}
