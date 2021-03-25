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

    protected bool isDead;

    public Sprite FaceSprite { get => faceSprite; }

    public event Action<Unit> OnHealthChange;

    public IController Controller { get; set; }
    public IMoveable Drive { get; set; }
    public Weapon Weapon { get; set; }
    public Vision Vision { get; set; }
    public VisionInterpreter VisionInterpreter { get; set; }
    public Equipment Equipment { get => equipment; }

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
}
