using System.Collections.Generic;
using UnityEngine;

public class FieldOfViewAgent : MonoBehaviour {

    private Color32 viewColor = new Color32(0x00, 0x00, 0xFF, 0xFF);

    [SerializeField]
    private bool generateFieldOfViewVectors = false;
    [SerializeField]
    private bool showFieldOfView = false;

    [SerializeField]
    private float angle = 90;
    [SerializeField]
    private float gapBetweenRaysExternal = 0.5f;
    [SerializeField]
    private float gapBetweenRaysInternal = 0.25f;

    private float angleBetweenRaysInExternalCircle = default;
    private float angleBetweenRaysInInternalCircle = default;
    [SerializeField]
    private float rayDistanceInExternalCircle = 8f;
    [SerializeField]
    private float rayDistanceInInternalCircle = 2f;
    [SerializeField]
    private Transform raysStartTransform = null;
    private int numberOfRaysInExternalCircle = 0;
    private int numberOfRaysInInternalCircle = 0;

    public List<GameObject> ObjectsInSight { get; private set; }
    public List<Vector3> FieldOfViewVectors { get; private set; }

    private void Start() {
        ObjectsInSight = new List<GameObject>();
        FieldOfViewVectors = new List<Vector3>();
        angleBetweenRaysInExternalCircle = Mathf.Rad2Deg * (gapBetweenRaysExternal / rayDistanceInExternalCircle);
        numberOfRaysInExternalCircle = Mathf.FloorToInt(angle / angleBetweenRaysInExternalCircle);
        angleBetweenRaysInInternalCircle = Mathf.Rad2Deg * (gapBetweenRaysInternal / rayDistanceInInternalCircle);
        numberOfRaysInInternalCircle = Mathf.FloorToInt((360f - angle) / angleBetweenRaysInInternalCircle);
    }

    private void FixedUpdate() {
        if (generateFieldOfViewVectors)
            FieldOfViewVectors.Clear();

        for (int i = 0; i < numberOfRaysInExternalCircle; i++) {
            Vector3 direction = Quaternion.Euler(0, (angle / -2f) + angleBetweenRaysInExternalCircle * i, 0) * transform.forward;
            Ray ray = new Ray(raysStartTransform.position, direction);
            RaycastHit[] hits = Physics.RaycastAll(ray, rayDistanceInExternalCircle);
            float distanceToClosestVisionBlocker = float.MaxValue;

            foreach (RaycastHit hit in hits) {
                Building spottedBuilding = hit.transform.GetComponent<Building>();
                if (spottedBuilding != null)
                    if (spottedBuilding.Viewable == false) 
                        if (Vector3.Distance(hit.transform.position, raysStartTransform.position) < distanceToClosestVisionBlocker) 
                            distanceToClosestVisionBlocker = Vector3.Distance(hit.transform.position, raysStartTransform.position);
            }

            if (generateFieldOfViewVectors)
                FieldOfViewVectors.Add(raysStartTransform.position + Quaternion.Euler(0, (angle / -2f) + angleBetweenRaysInExternalCircle * i, 0) * transform.forward * Mathf.Min(distanceToClosestVisionBlocker, rayDistanceInExternalCircle));

            foreach (RaycastHit hit in hits) {
                if (Vector3.Distance(hit.transform.position, raysStartTransform.position) <= distanceToClosestVisionBlocker) {
                    if (ObjectsInSight.Contains(hit.transform.gameObject) == false)
                        ObjectsInSight.Add(hit.transform.gameObject);
                }
            }
        }

        for (int i = 0; i < numberOfRaysInInternalCircle; i++) {
            Vector3 direction = Quaternion.Euler(0, (angle / 2f) + angleBetweenRaysInInternalCircle * i, 0) * transform.forward;
            Ray ray = new Ray(raysStartTransform.position, direction);
            RaycastHit[] hits = Physics.RaycastAll(ray, rayDistanceInInternalCircle);
            float distanceToClosestVisionBlocker = float.MaxValue;

            foreach (RaycastHit hit in hits) {
                Building spottedBuilding = hit.transform.GetComponent<Building>();
                if (spottedBuilding != null)
                    if (spottedBuilding.Viewable == false) 
                        if (Vector3.Distance(hit.transform.position, raysStartTransform.position) < distanceToClosestVisionBlocker) 
                            distanceToClosestVisionBlocker = Vector3.Distance(hit.transform.position, raysStartTransform.position);
            }

            if (generateFieldOfViewVectors)
                FieldOfViewVectors.Add(raysStartTransform.position + Quaternion.Euler(0, (angle / 2f) + angleBetweenRaysInInternalCircle * i, 0) * transform.forward * Mathf.Min(distanceToClosestVisionBlocker, rayDistanceInInternalCircle));

            foreach (RaycastHit hit in hits) {
                if (Vector3.Distance(hit.transform.position, raysStartTransform.position) <= distanceToClosestVisionBlocker) {
                    if (ObjectsInSight.Contains(hit.transform.gameObject) == false)
                        ObjectsInSight.Add(hit.transform.gameObject);
                }
            }
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = viewColor;
        Vector3 center = raysStartTransform.position;

        if (showFieldOfView) {
            if (generateFieldOfViewVectors) {
                foreach (Vector3 vectorEnd in FieldOfViewVectors) {
                    Gizmos.DrawLine(center, vectorEnd);
                }
            }
        }
    }
}
