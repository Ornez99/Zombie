using UnityEngine;

public interface IMoveable {

    Unit Unit { get; set; }
    float Speed { get; set; }
    bool DestinationReached { get; }
    bool PathCreated { get; }
    void CreateAndSetPathToPosition(Vector3 position);
    void MoveWithNormalizedDirection(Vector3 normalizedDirection);
    void MoveWithPath();
    void ResetPath();

}
