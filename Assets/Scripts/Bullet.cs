using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    [SerializeField]
    private float damage = 3.5f;

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Wall") {
            Destroy(gameObject);
        }
        else if (other.GetComponent<Unit>() != null) {
            if (other.GetComponent<Unit>().Controller.ToString() == "EnemyController") {
                Destroy(gameObject);
                other.GetComponent<Unit>().TakeDamge(damage);
            }
        }
    }
}
