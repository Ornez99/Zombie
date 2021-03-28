using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    [SerializeField]
    private float speed;

    [SerializeField]
    private Transform cameraTransform;

    private void Update() {
        Vector3 inputDir = Quaternion.Euler(0, 45, 0) * new Vector3(Input.GetAxis("Horizontal") * Time.deltaTime * speed, 0, Input.GetAxis("Vertical") * Time.deltaTime * speed);
        Vector3 newPos = cameraTransform.position + inputDir;
        cameraTransform.position = newPos;
    }
}
