using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour {

    [SerializeField]
    protected int team;
    [SerializeField]
    protected Transform hand;
    [SerializeField]
    protected Transform mouth;
    [SerializeField]
    protected Animator animator;
    [SerializeField]
    protected Sprite faceSprite;
    [SerializeField]
    protected CapsuleCollider capsuleCollider;

    [SerializeField]
    protected Equipment equipment;

    [SerializeField]
    protected List<GameObject> Graphics;

    protected bool isDead;

    public Sprite FaceSprite { get => faceSprite; }

    public IController Controller { get; set; }
    public IMoveable Drive { get; set; }
    public IFieldOfView FieldOfView { get; set; }

    public Weapon Weapon { get; set; }
    public Equipment Equipment { get => equipment; }

    public Transform Hand { get => hand; set => hand = value; }
    public Transform Mouth { get => mouth; set => mouth = value; }
    public int GetTeam { get => team; }
    public Node Node { get; set; }
    public Animator Animator { get => animator; }

    protected void Awake() {
        Drive = GetComponent<IMoveable>();
        Drive.Unit = this;
        Node = Map.GetNodeFromPos(transform.position);
    }
}
