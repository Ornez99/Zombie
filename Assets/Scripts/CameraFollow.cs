using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    private Vector3 velocity;
    private Transform target;

    [SerializeField]
    private Transform camTransform = null;
    private Vector3 offset;
    public float SmoothTime = 0.3f;

    public void SetCameraTarget(Transform target) {
        camTransform.transform.position = target.transform.position + new Vector3(0, 7, -7);
        this.target = target;
        offset = camTransform.position - this.target.position;
    }

    private void LateUpdate() {
        if (target == null)
            return;

        Vector3 targetPosition = target.position + offset;
        camTransform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, SmoothTime);

        transform.LookAt(target);
    }
}
