using System.Collections.Generic;
using UnityEngine;

public class Vision : MonoBehaviour {

    //private Color32 gizmosColorForObjectInSight;
    //private Color32 gizmosColorForObjectInSightVisionBlocker;
    private Vector3 gizmosSizeForObjectInSight;
    private int numberOfRays;
    private float gapBetweenRays;
    private float angleBetweenRays;
    [SerializeField]
    private float rayDistance = 0;
    [SerializeField]
    private Transform raysStartTransform = null;
    [SerializeField]
    private bool showObjectsInSight = false;
    [SerializeField]
    private float visionAngle = 0;

    public List<GameObject> ObjectsInSight { get; private set; }

    private void Awake() {
        //gizmosColorForObjectInSight = new Color32(0x2F, 0x8B, 0x1C, 0xFF);
        //gizmosColorForObjectInSightVisionBlocker = new Color32(0xAC, 0x2C, 0x14, 0xFF);
        gizmosSizeForObjectInSight = new Vector3(0.3f, 2f, 0.3f);

        gapBetweenRays = 0.5f;
        ObjectsInSight = new List<GameObject>();
        angleBetweenRays = Mathf.Rad2Deg * (gapBetweenRays / rayDistance);
        numberOfRays = Mathf.FloorToInt(visionAngle / angleBetweenRays);
    }

    private void FixedUpdate() {
        Tick();
    }

    public void Tick() {
        ObjectsInSight.Clear();
        for (int i = 0; i < numberOfRays; i++) {
            RaycastHit[] hits = Physics.RaycastAll(raysStartTransform.position, Quaternion.Euler(0, (visionAngle / -2f) + angleBetweenRays * i, 0) * raysStartTransform.forward, rayDistance);
            float distanceToClosestVisionBlocker = float.MaxValue;
            foreach (RaycastHit hit in hits) {
                Building spottedBuilding = hit.transform.GetComponent<Building>();
                if (spottedBuilding != null)
                    if (spottedBuilding.Viewable == false) {
                        if (Vector3.Distance(hit.transform.position, raysStartTransform.position) < distanceToClosestVisionBlocker) {
                            distanceToClosestVisionBlocker = Vector3.Distance(hit.transform.position, raysStartTransform.position);
                        }
                    }
            }
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
