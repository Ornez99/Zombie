﻿using System.Collections;
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
    [SerializeField]
    private Transform zombieModel;

    public float MaxHealth { get => maxHealth; set => maxHealth = value; }
    public float CurrentHealth { get => currentHealth; set => currentHealth = value; }
    public float Armor { get => armor; set => armor = value; }
    public ZombieSpawner ZombieSpawner { get; set; }

    private void Update() {
        if (isDead)
            return;
        
        zombieModel.localPosition = Vector3.zero; // Animations are breaking game :(

        Controller.Tick();

        SetActiveGraphics(Map.Instance.Grid[Node.XId - 1, Node.YId - 1]?.Visible == true);
    }

    private void LateUpdate() {
        FieldOfView?.Tick();
    }

#if UNITY_EDITOR
    private void OnGUI() {
        //Handles.Label(transform.position, Controller.StateMachine.ToString());
    }
#endif
    private void OnDrawGizmos() {
        if (Node == null)
            return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(Node.CenterPos, Vector3.one);
    }

    private void OnDestroy() {
        ZombieSpawner?.SpawnedZombies.Remove(this);
    }

    public void TakeDamage(float amount) {
        currentHealth -= amount;
        CheckIfShouldBeDead();
    }

    public void Heal(float amount) {
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
    }

    private void SetActiveGraphics(bool value) {
        for (int i = Graphics.Count - 1; i >= 0; i--) {
            Graphics[i].SetActive(value);
        }
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
