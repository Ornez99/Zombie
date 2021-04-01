using UnityEditor;
using UnityEngine;

public class StateLabel : MonoBehaviour {

    [SerializeField]
    private IController controller;

    private void Start() {
        controller = GetComponent<Unit>().Controller;
    }

#if UNITY_EDITOR
    private void OnGUI()
    {
        if (controller != null)
            Handles.Label(transform.position, controller.StateMachine?.ToString());
    }
#endif
}
