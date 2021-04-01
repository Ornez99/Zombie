using System.Collections.Generic;
using UnityEngine;

public class FieldOfViewAlly : IFieldOfView {

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

    public List<GameObject> VisibleObjects { get; private set; }
    public Vector3[] Vectors { get; private set; }
    public IInteractable ClosestInteractable { get; private set; }
    public Transform RaysStartTransform => raysStartTransform;
    public bool ShowFieldOfView { get => showFieldOfView; set => showFieldOfView = value; }
    public Unit ClosestEnemy { get; private set; }

    public FieldOfViewAlly(Unit owner, float rayDistance, float angle, Transform raysStartTransform) {
        this.owner = owner;
        this.rayDistance = rayDistance;
        this.angle = angle;
        this.raysStartTransform = raysStartTransform;
        VisibleObjects = new List<GameObject>();

        angleBetweenRays = Mathf.Rad2Deg * (gapBetweenRays / rayDistance);
        numberOfRays = Mathf.FloorToInt(angle / angleBetweenRays);
        Vectors = new Vector3[numberOfRays];
        startAngle = angle / -2f;
    }

    public void Tick() {
        ClosestInteractable = null;
        VisibleObjects.Clear();

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

            float distToClosestEnemy = float.MaxValue;
            foreach (RaycastHit hit in hits) {
                if (Vector3.Distance(hit.transform.position, raysStartTransform.position) <= distanceToClosestVisionBlocker) {
                    if (hit.transform.GetComponent<IInteractable>() != null)
                        ClosestInteractable = hit.transform.GetComponent<IInteractable>();

                    VisibleObjects.Add(hit.transform.gameObject);

                    Unit potentialClosestEnemy = hit.transform.GetComponent<Unit>();
                    if(potentialClosestEnemy != null)
                        if (potentialClosestEnemy.Team != owner.Team)
                            if (Vector3.Distance(hit.transform.position, owner.transform.position) < distToClosestEnemy) {
                            
                                ClosestEnemy = potentialClosestEnemy;
                                distToClosestEnemy = Vector3.Distance(hit.transform.position, owner.transform.position);
                            }
                }
            }

            if (showFieldOfView)
                Debug.DrawLine(raysStartTransform.position, Vectors[i]);
        }
    }
}
