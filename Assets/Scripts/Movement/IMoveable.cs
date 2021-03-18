using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveable {

    Node Node { get; }
    bool NodeChanged { get; }
    bool PathFound { get; }
    bool DestinationReached { get; }
    void CreatePathToPosition(Vector3 position);
    void Move();
    void Translate(Vector3 normalVector3);
    void SetSpeed(float speed);
}
