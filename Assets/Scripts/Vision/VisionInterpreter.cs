using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionInterpreter {

    private Unit unit;
    private Vision vision;

    public Unit ClosestEnemy { get; private set; }
    public IInteractable ClosestInteractable { get; private set; }

    public VisionInterpreter(Vision vision, Unit unit) {
        this.vision = vision;
        this.unit = unit;
    }

    public void Tick() {
        ClosestInteractable = null;
        ClosestEnemy = null;
        float minDistToClosestEnemy = float.MaxValue;
        float minDistToClosestInteractable = float.MaxValue;

        List<GameObject> objectsInSight = vision.ObjectsInSight;
        foreach (GameObject go in objectsInSight) {
            if (go == null)
                continue;

            if (go.GetComponent<Unit>() != null) {
                Unit potentialUnit = go.GetComponent<Unit>();
                if (potentialUnit.GetTeam != unit.GetTeam) {
                    float potentialDistance = Vector3.Distance(go.transform.position, unit.transform.position);
                    if (minDistToClosestEnemy > potentialDistance) {
                        minDistToClosestEnemy = potentialDistance;
                        ClosestEnemy = potentialUnit;
                    }
                }
            }
            else if (go.GetComponent<IInteractable>() != null) {
                if (go.GetComponent<IInteractable>().Enabled) {
                    float potentialDistance = Vector3.Distance(go.transform.position, unit.transform.position);
                    if (minDistToClosestInteractable > potentialDistance) {
                        minDistToClosestInteractable = potentialDistance;
                        ClosestInteractable = go.GetComponent<IInteractable>(); ;
                    }
                }
            }
        }
    }
}
