﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour {

    private Node node;
    [SerializeField]
    protected int team;
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

    public IController Controller { get; set; }
    public IMoveable Drive { get; set; }
    public Weapon Weapon { get; set; }
    public Vision Vision { get; set; }
    public VisionInterpreter VisionInterpreter { get; set; }

    public Transform Hand { get => hand; set => hand = value; }
    public Transform Mouth { get => mouth; set => mouth = value; }
    public int GetTeam { get => team; }
    public Node Node { get => node; protected set => node = value; }
    public Animator Animator { get => animator; }

    private void Awake() {
        Vision = GetComponent<Vision>();
        VisionInterpreter = new VisionInterpreter(Vision, this);
        Drive = GetComponent<IMoveable>();
        Node = Map.GetNodeFromPos(transform.position);
    }

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
