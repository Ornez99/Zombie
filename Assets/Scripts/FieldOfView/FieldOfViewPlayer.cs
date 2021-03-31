using System.Collections.Generic;
using UnityEngine;

public class FieldOfViewPlayer : IFieldOfView {

    private static Color32 raysColor = new Color32(0x00, 0x00, 0xFF, 0xFF);

    [SerializeField]
    private bool showFieldOfView = false;

    [SerializeField]
    private float angle = 90f;

    [SerializeField]
    private float gapBetweenRays = 0.5f;

    [SerializeField]
    private float rayDistance = 5f;

    [SerializeField]
    private Transform raysStartTransform = null;

    private float angleBetweenRays = default;
    private float startAngle = default;
    private int numberOfRays = default;
    private Unit owner = null;

    public List<GameObject> VisibleObjects { get => null; }
    public Vector3[] Vectors { get; private set; }
    public IInteractable ClosestInteractable { get; private set; }
    public Transform RaysStartTransform => raysStartTransform;
    public bool ShowFieldOfView { get => showFieldOfView; set => showFieldOfView = value; }
    public Unit ClosestEnemy { get => null; }

    public FieldOfViewPlayer(Unit owner, float rayDistance, float angle, Transform raysStartTransform) {
        this.owner = owner;
        this.rayDistance = rayDistance;
        this.angle = angle;
        this.raysStartTransform = raysStartTransform;

        angleBetweenRays = Mathf.Rad2Deg * (gapBetweenRays / rayDistance);
        numberOfRays = Mathf.FloorToInt(angle / angleBetweenRays);
        Vectors = new Vector3[numberOfRays];
        startAngle = angle / -2f;
    }

    public void Tick() {
        ClosestInteractable = null;
        float minDistToInteractable = float.MaxValue;

        for (int i = 0; i < numberOfRays; i++) {
            Vector3 direction = Quaternion.Euler(0, startAngle + angleBetweenRays * i, 0) * raysStartTransform.forward;
            RaycastHit[] hits = Physics.RaycastAll(raysStartTransform.position, direction, rayDistance);

            float distanceToClosestVisionBlocker = rayDistance;
            float dist = float.MaxValue;

            foreach (RaycastHit hit in hits) {
                Building spottedBuilding = hit.transform.GetComponent<Building>();

                if (spottedBuilding?.Viewable == false) {
                    dist = Vector3.Distance(hit.transform.position, raysStartTransform.position);
                    if (dist < distanceToClosestVisionBlocker)
                        distanceToClosestVisionBlocker = dist;
                }
            }
            
            Vectors[i] = raysStartTransform.position + direction * distanceToClosestVisionBlocker;


            foreach (RaycastHit hit in hits) {
                if (Vector3.Distance(hit.transform.position, raysStartTransform.position) <= distanceToClosestVisionBlocker) {
                    if (hit.transform.GetComponent<IInteractable>() != null && hit.transform.GetComponent<IInteractable>()?.Enabled == true) {
                        float potentialDist = Vector3.Distance(owner.transform.position, hit.transform.position);
                        if (minDistToInteractable > potentialDist && potentialDist < 1.5f) {
                            minDistToInteractable = potentialDist;
                            ClosestInteractable = hit.transform.GetComponent<IInteractable>();
                        }
                    }
                }
            }

            if (showFieldOfView)
                Debug.DrawLine(raysStartTransform.position, Vectors[i]);
        }
    }
}
