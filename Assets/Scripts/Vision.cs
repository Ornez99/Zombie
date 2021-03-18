using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vision {

    private const float lessThanHalfOfUnitSize = 0.45f;

    private int layerAffectedByVisionRay;

    [SerializeField]
    private Unit unit;
    [SerializeField]
    private Transform unitTransform;
    [SerializeField]
    private int numberOfRays;
    [SerializeField]
    private float anglePerRay;
    [SerializeField]
    private float viewDistance;
    [SerializeField]
    private Unit closestEnemy;

    public bool EnemySpotted { get => closestEnemy != null; }
    public Unit ClosestEnemy { get => closestEnemy; }

    public Vision(Transform unitTransform, float viewDistance) {
        this.unitTransform = unitTransform;
        this.viewDistance = viewDistance;
        unit = unitTransform.GetComponent<Unit>();
        
        layerAffectedByVisionRay = LayerMask.GetMask("AffectedByVisionRay");

        anglePerRay = Mathf.Rad2Deg * (lessThanHalfOfUnitSize / viewDistance) * 2f;
        numberOfRays = Mathf.FloorToInt(360 / anglePerRay);
    }

    public void Tick() {
        closestEnemy = null;
        float minDist = float.MaxValue;

        for (int i = 0; i < numberOfRays; i++) {
            RaycastHit hit;
            if (Physics.Raycast(unitTransform.position + new Vector3(0,1f,0), Quaternion.Euler(0, i * anglePerRay, 0) * Vector3.forward, out hit, viewDistance, layerAffectedByVisionRay)) {
                //Debug.DrawLine(unitTransform.position + new Vector3(0, 0.5f, 0), hit.point, Color.blue);

                Unit potentialUnit = hit.transform.GetComponent<Unit>();
                if (potentialUnit == null)
                    continue;

                if (potentialUnit.GetTeam != unit.GetTeam) {
                    if (closestEnemy == null)
                        closestEnemy = hit.transform.GetComponent<Unit>();
                    else if (Vector3.Distance(unitTransform.position, hit.transform.position) < minDist) {
                        minDist = Vector3.Distance(unitTransform.position, hit.transform.position);
                        closestEnemy = hit.transform.GetComponent<Unit>();
                    } 
                }
            }
        }
        if (closestEnemy != null && closestEnemy.GetTeam == 0)
            Debug.DrawLine(unitTransform.position + new Vector3(0, 1f, 0), closestEnemy.transform.position + new Vector3(0, 1f, 0), Color.red);
    }
}
