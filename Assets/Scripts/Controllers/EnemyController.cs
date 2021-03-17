using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : IController {
    public Unit Owner { get; private set; }

    public EnemyController(Unit unit) {
        Owner = unit;
    }

    public void Tick() {
        Node currentNode = Owner.Drive.Node;
        Vector2 vector2 = SmellManager.Instance.VectorMap[currentNode.XId - 1,currentNode.YId - 1];
        if (Mathf.Abs(vector2.x) > 0.2f || Mathf.Abs(vector2.y) > 0.2f) {
            Vector3 normalVector3 = new Vector3(vector2.x, 0, vector2.y).normalized;
            Owner.Drive.Translate(normalVector3);
        }
    }

    public override string ToString() {
        return "EnemyController";
    }

}
