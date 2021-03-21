using UnityEngine;

public interface IMoveable {

    bool DestinationReached { get; }
    bool PathCreated { get; }
    void CreateAndSetPathToPosition(Vector3 position);
    void MoveWithNormalizedDirection(Vector3 normalizedDirection);
    void MoveWithPath();
    void SetSpeed(float newSpeed);
    void ResetPath();

}
