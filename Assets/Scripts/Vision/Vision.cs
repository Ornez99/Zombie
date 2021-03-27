using System.Collections.Generic;
using UnityEngine;

public class Vision : MonoBehaviour {

    private Vector3 gizmosSizeForObjectInSight;
    private int numberOfRays;
    [SerializeField]
    private float gapBetweenRays;
    private float angleBetweenRays;
    [SerializeField]
    private float rayDistance = default;
    [SerializeField]
    private Transform raysStartTransform = default;
    [SerializeField]
    private bool showObjectsInSight = default;
    [SerializeField]
    private float visionAngle = default;
    [SerializeField]
    private bool saveHitsVector3 = default;

    public Transform Origin { get => raysStartTransform; }
    public List<GameObject> ObjectsInSight { get; private set; }
    public List<Vector3> HitsVector3 { get; private set; }

    private void Awake() {
        gizmosSizeForObjectInSight = new Vector3(0.3f, 2f, 0.3f);
        ObjectsInSight = new List<GameObject>();
        angleBetweenRays = Mathf.Rad2Deg * (gapBetweenRays / rayDistance);
        numberOfRays = Mathf.FloorToInt(visionAngle / angleBetweenRays);
        HitsVector3 = new List<Vector3>();
    }

    private void FixedUpdate() {
        Tick();
    }

    public void Tick() {
        ObjectsInSight.Clear();

        if (saveHitsVector3)
            HitsVector3.Clear();

        for (int i = 0; i < numberOfRays; i++) {
            RaycastHit[] hits = Physics.RaycastAll(raysStartTransform.position, Quaternion.Euler(0, (visionAngle / -2f) + angleBetweenRays * i, 0) * transform.forward, rayDistance);
            float distanceToClosestVisionBlocker = float.MaxValue;

            Vector3 FarthestObjectHitPoint = default;
            if(saveHitsVector3)
                FarthestObjectHitPoint = raysStartTransform.position + (Quaternion.Euler(0, (visionAngle / -2f) + angleBetweenRays * i, 0) * transform.forward) * rayDistance;

            foreach (RaycastHit hit in hits) {
                Building spottedBuilding = hit.transform.GetComponent<Building>();
                if (spottedBuilding != null)
                    if (spottedBuilding.Viewable == false) {
                        if (Vector3.Distance(hit.transform.position, raysStartTransform.position) < distanceToClosestVisionBlocker) {
                            distanceToClosestVisionBlocker = Vector3.Distance(hit.transform.position, raysStartTransform.position);

                            if (saveHitsVector3)
                                FarthestObjectHitPoint = hit.point;
                        }
                    }

            }

            if (saveHitsVector3)
                HitsVector3.Add(FarthestObjectHitPoint);

            foreach (RaycastHit hit in hits) {
                if (Vector3.Distance(hit.transform.position, raysStartTransform.position) <= distanceToClosestVisionBlocker) {
                    if (ObjectsInSight.Contains(hit.transform.gameObject) == false)
                        ObjectsInSight.Add(hit.transform.gameObject);
                }
            }
        }
    }

    private void OnDrawGizmos() {
        if (showObjectsInSight) {
            Gizmos.color = Color.red;
            foreach (GameObject go in ObjectsInSight) {
                Gizmos.DrawWireCube(go.transform.position + (Vector3.up * gizmosSizeForObjectInSight.y / 2f), gizmosSizeForObjectInSight);
            }
        }
    }

}
