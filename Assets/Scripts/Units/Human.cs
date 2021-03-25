using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Unit, IKillable {

    [SerializeField]
    private int smellValue = 0;
    private RectTransform healthTransform;

    [SerializeField]
    private float maxHealth;
    [SerializeField]
    private float currentHealth;
    [SerializeField]
    private float armor;

    public float MaxHealth { get => maxHealth; set => maxHealth = value; }
    public float CurrentHealth { get => currentHealth; set => currentHealth = value; }
    public float Armor { get => armor; set => armor = value; }

    private void FixedUpdate() {
        if (isDead)
            return;

        Controller.Tick();

        VisionInterpreter.Tick();

        Node = Map.GetNodeFromPos(transform.position);
        if (Node.SmellValue < smellValue)
            Node.SmellValue = smellValue;
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }

    public void OnTakeControl(PlayerController playerController) {
        Controller = playerController;
        Animator.SetBool("Run", false);
        Animator.SetBool("Walk", false);
        Animator.SetBool("RangedAttack", false);
        Equipment.UpdateUI();
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
