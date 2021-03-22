using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour {

    [SerializeField]
    protected int team;
    [SerializeField]
    protected float damagedTimer;
    [SerializeField]
    protected float maxHealth;
    [SerializeField]
    protected float currentHealth;
    [SerializeField]
    protected Transform hand;
    [SerializeField]
    protected Transform mouth;
    [SerializeField]
    protected Animator animator;
    [SerializeField]
    protected Sprite faceSprite;

    [SerializeField]
    protected Equipment equipment;

    protected bool isDead;

    public Sprite FaceSprite { get => faceSprite; }

    public event Action<Unit> OnHealthChange;

    public IController Controller { get; set; }
    public IMoveable Drive { get; set; }
    public Weapon Weapon { get; set; }
    public Vision Vision { get; set; }
    public VisionInterpreter VisionInterpreter { get; set; }
    public Equipment Equipment { get => equipment; }

    public float MaxHealth { get => maxHealth; }
    public float CurrentHealth { get => currentHealth; }

    public Transform Hand { get => hand; set => hand = value; }
    public Transform Mouth { get => mouth; set => mouth = value; }
    public int GetTeam { get => team; }
    public Node Node { get; protected set; }
    public Animator Animator { get => animator; }
    

    protected void Awake() {
        Vision = GetComponent<Vision>();
        VisionInterpreter = new VisionInterpreter(Vision, this);
        Drive = GetComponent<IMoveable>();
        Node = Map.GetNodeFromPos(transform.position);
    }



    public void TakeDamge(float amount) {
        RemoveHealth(amount);
        CheckIfShouldBeDead();
        OnHealthChange?.Invoke(this);
    }

    private void RemoveHealth(float amount) {
        currentHealth -= amount;
    }

    private void CheckIfShouldBeDead() {
        if (currentHealth <= 0) {
            isDead = true;
            GetComponent<CapsuleCollider>().enabled = false;
            animator.SetBool("Death", true);
            Destroy(gameObject, 1f);
        }

    }
}
