using UnityEngine;

public class NodeDisplayer : MonoBehaviour
{

    [SerializeField]
    private Unit unit;

    private void Start() {
        unit = GetComponent<Unit>();
    }

    private void OnDrawGizmos() {
        if (unit.Node != null) {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(unit.Node.CenterPos, Vector3.one);
        }
    }
}
