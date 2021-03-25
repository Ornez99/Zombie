using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Zombie : Unit, IKillable {

    [SerializeField]
    private Transform visionRaysStartTransform;

    [SerializeField]
    private float maxHealth;
    [SerializeField]
    private float currentHealth;
    [SerializeField]
    private float armor;

    public float MaxHealth { get => maxHealth; set => maxHealth = value; }
    public float CurrentHealth { get => currentHealth; set => currentHealth = value; }
    public float Armor { get => armor; set => armor = value; }

    private void Update() {
        if (isDead)
            return;

        transform.position = new Vector3(transform.position.x, 0, transform.position.z);

        Node = Map.GetNodeFromPos(transform.position);
        Controller.Tick();
        VisionInterpreter.Tick();
        Node = Map.GetNodeFromPos(transform.position);
    }

    private void FixedUpdate() {
        Vision?.Tick();
    }

#if UNITY_EDITOR
    private void OnGUI() {
        Handles.Label(transform.position, Controller.StateMachine.ToString());
    }
#endif
    private void OnDrawGizmos() {
        if (Node == null)
            return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(Node.CenterPos, Vector3.one);
    }

    public void TakeDamage(float amount) {
        currentHealth -= amount;
        CheckIfShouldBeDead();
    }

    public void Heal(float amount) {
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
    }

    private void CheckIfShouldBeDead() {
        if (currentHealth <= 0) {
            isDead = true;
            capsuleCollider.enabled = false;
            animator.SetBool("Death", true);
            Destroy(gameObject, 1f);
        }
    }
}
