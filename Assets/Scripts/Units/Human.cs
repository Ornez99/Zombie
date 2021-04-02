using System;
using UnityEditor;
using UnityEngine;

public class Human : Unit, IKillable
{

    [SerializeField]
    private Transform humanModel;

    [SerializeField]
    private float currentHealth;

    [SerializeField]
    private float armor;

    public event Action<Unit> OnHealthChange;

    public float MaxHealth { get => unitData.MaxHealth; }
    public float CurrentHealth { get => currentHealth; set => currentHealth = value; }
    public float Armor { get => armor; set => armor = value; }

    private void Start()
    {
        FieldOfView.ShowFieldOfView = true;
    }

    private void Update()
    {
        if (isDead)
            return;

        humanModel.localPosition = Vector3.zero; // Animations are breaking game

        Controller.Tick();
    }

    private void LateUpdate()
    {
        FieldOfView?.Tick();
    }

    public void OnTakeControl(PlayerController playerController)
    {
        Controller = playerController;
        FieldOfView = new FieldOfViewPlayer(this, 10f, 90f, transform.GetChild(1));
        Animator.SetBool("Run", false);
        Animator.SetBool("Walk", false);
        Animator.SetBool("RangedAttack", false);
        equipmentUI.UpdateItemsUI(equipment);
        FieldOfView.ShowFieldOfView = true;
    }
    
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        CheckIfShouldBeDead();
        OnHealthChange?.Invoke(this);
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(unitData.MaxHealth, currentHealth + amount);
        OnHealthChange?.Invoke(this);
    }

    private void CheckIfShouldBeDead()
    {
        if (currentHealth <= 0)
        {
            isDead = true;
            capsuleCollider.enabled = false;
            animator.SetBool("Death", true);
            Destroy(gameObject, 1f);
        }
    }

    private void OnDestroy()
    {
        Player.Instance.RemoveOwnedHuman(this);
        if (Player.Instance.GetOwnedHumansCount() == 0)
            MainQuest.Instance.QuestLost();
    }
}
