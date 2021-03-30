using System.Collections.Generic;
using UnityEngine;

public class FieldOfViewEnemy : IFieldOfView {

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
    public Vector3[] Vectors { get => null; }
    public IInteractable ClosestInteractable { get => null; }
    public Transform RaysStartTransform => raysStartTransform;
    public bool ShowFieldOfView { get => showFieldOfView; set => showFieldOfView = value; }
    public Unit ClosestEnemy { get; private set; }

    public FieldOfViewEnemy(Unit owner, float rayDistance, float angle, Transform raysStartTransform) {
        this.owner = owner;
        this.rayDistance = rayDistance;
        this.angle = angle;
        this.raysStartTransform = raysStartTransform;
        VisibleObjects = new List<GameObject>();

        angleBetweenRays = Mathf.Rad2Deg * (gapBetweenRays / rayDistance);
        numberOfRays = Mathf.FloorToInt(angle / angleBetweenRays);
        startAngle = angle / -2f;
    }

    public void Tick() {
        VisibleObjects.Clear();
        ClosestEnemy = null;

        for (int i = 0; i < numberOfRays; i++) {
            Vector3 direction = Quaternion.Euler(0, startAngle + angleBetweenRays * i, 0) * raysStartTransform.forward;
            RaycastHit hit;

            float distToClosestEnemy = float.MaxValue;
            if (Physics.Raycast(raysStartTransform.position, direction, out hit, rayDistance)) {
                if (VisibleObjects.Contains(hit.transform.gameObject) == false)
                    VisibleObjects.Add(hit.transform.gameObject);

                Unit potentialClosestEnemy = hit.transform.GetComponent<Unit>();
                if(potentialClosestEnemy != null)
                    if (potentialClosestEnemy.GetTeam != owner.GetTeam)
                        if (Vector3.Distance(hit.transform.position, owner.transform.position) < distToClosestEnemy) {
                            ClosestEnemy = potentialClosestEnemy;
                            distToClosestEnemy = Vector3.Distance(hit.transform.position, owner.transform.position);
                        }
            }

            if (showFieldOfView) {
                float dist = rayDistance;
                if (hit.point != Vector3.zero) {
                    dist = Vector3.Distance(raysStartTransform.position, hit.point);
                }
                    
                
                Debug.DrawLine(raysStartTransform.position, raysStartTransform.position + direction * dist);
            }
        }
    }
}