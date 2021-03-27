using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Human : Unit, IKillable {

    private RectTransform healthTransform;

    [SerializeField]
    private float maxHealth;
    [SerializeField]
    private float currentHealth;
    [SerializeField]
    private float armor;

    public event Action<Unit> OnHealthChange;

    public float MaxHealth { get => maxHealth; set => maxHealth = value; }
    public float CurrentHealth { get => currentHealth; set => currentHealth = value; }
    public float Armor { get => armor; set => armor = value; }

    private void FixedUpdate() {
        if (isDead)
            return;

        Controller.Tick();

        VisionInterpreter.Tick();

        Node = Map.GetNodeFromPos(transform.position);
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }

    public void OnTakeControl(PlayerController playerController) {
        Controller = playerController;
        Animator.SetBool("Run", false);
        Animator.SetBool("Walk", false);
        Animator.SetBool("RangedAttack", false);
        Equipment.UpdateUI();
    }

    private void OnTakeDamage() {

    }
    
    public void TakeDamage(float amount) {
        currentHealth -= amount;
        CheckIfShouldBeDead();
        OnHealthChange?.Invoke(this);
    }

    public void Heal(float amount) {
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        OnHealthChange?.Invoke(this);
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
