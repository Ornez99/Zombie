using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour {

    [SerializeField]
    protected int team;
    [SerializeField]
    protected float maxHealth;
    [SerializeField]
    protected float currentHealth;
    [SerializeField]
    protected Transform hand;

    public IController Controller { get; set; }
    public IMoveable Drive{ get; set; }
    public Weapon Weapon { get; set; }
    public Vision Vision { get; set; }
    public Transform Hand { get => hand; set => hand = value; }
    public int GetTeam { get => team; }

    public void OnTakeControl(PlayerController playerController) {
        Controller = playerController;
    }

    public void TakeDamge(float amount) {
        currentHealth -= amount;
        if (currentHealth <= 0) {
            Destroy(gameObject);
        }
    }
}
