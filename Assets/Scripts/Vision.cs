using System.Collections.Generic;
using UnityEngine;

public class Vision : MonoBehaviour {

    private Color32 raysHitColor;
    private Color32 raysNoHitColor;
    private int numberOfRays;
    private float gapBetweenRays;
    private float angleBetweenRays;
    [SerializeField]
    private float rayDistance = 0;
    [SerializeField]
    private Transform raysStartTransform = null;
    [SerializeField]
    private bool showRays = false;
    [SerializeField]
    private float visionAngle = 0;

    public bool ShowRays { get => showRays; set => value = showRays; }
    public List<GameObject> ObjectsInSight { get; private set; }

    private void Awake() {
        gapBetweenRays = 0.5f;
        raysHitColor = new Color32(0x2F, 0x8B, 0x1C, 0xFF);
        raysNoHitColor = new Color32(0xAC, 0x2C, 0x14, 0xFF);

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
            RaycastHit hit;
            if (Physics.Raycast(raysStartTransform.position, Quaternion.Euler(0, (visionAngle / -2f) + angleBetweenRays * i, 0) * raysStartTransform.forward, out hit, rayDistance)) {
                if (ObjectsInSight.Contains(hit.transform.gameObject) == false) {
                    ObjectsInSight.Add(hit.transform.gameObject);
                }
            }
            if (ShowRays) {
                if (hit.transform != null)
                    Debug.DrawLine(raysStartTransform.position, hit.point, raysHitColor);
                else
                    Debug.DrawLine(raysStartTransform.position, raysStartTransform.position + Quaternion.Euler(0, (visionAngle / -2f) + angleBetweenRays * i, 0) * raysStartTransform.forward, raysNoHitColor);
            }
        }
    }

    public List<GameObject> GetObjectsInSightWithName(string name) {
        List<GameObject> objs = new List<GameObject>();
        foreach (GameObject go in ObjectsInSight) {
            if (go?.name == name)
                objs.Add(go);
        }
        return objs;
    }

}
