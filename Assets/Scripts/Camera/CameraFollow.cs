using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    private Transform target;
    private float smoothing = 5f;

    private Vector3 offset;

    public void SetCameraTarget(Transform target) {
        transform.position = target.transform.position + new Vector3(-17, 15, -17);
        this.target = target;
        offset = transform.position - target.position;
    }

    private void LateUpdate() {
        Vector3 targetCamPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
    }
}
