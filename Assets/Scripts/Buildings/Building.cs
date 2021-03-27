using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour {

    [SerializeField]
    private bool walkable;
    public bool Walkable { get => walkable; protected set => walkable = value; }

    [SerializeField]
    private bool buildable;
    public bool Buildable { get => buildable; protected set => buildable = value; }

    [SerializeField]
    private bool viewable;
    public bool Viewable { get => viewable; protected set => viewable = value; }

    [SerializeField]
    private bool smellable;
    public bool Smellable { get => smellable; protected set => smellable = value; }
}
