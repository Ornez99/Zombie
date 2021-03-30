using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFieldOfView {

    List<GameObject> VisibleObjects { get; }
    Vector3[] Vectors { get; }
    IInteractable ClosestInteractable { get; }
    Transform RaysStartTransform { get; }
    bool ShowFieldOfView { get; set; }
    Unit ClosestEnemy { get; }

    void Tick();
}
