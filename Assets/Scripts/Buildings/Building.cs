using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour {

    [SerializeField]
    private bool walkable;
    public bool Walkable { get => walkable; private set => walkable = value; }

    [SerializeField]
    private bool buildable;
    public bool Buildable { get => buildable; private set => buildable = value; }

    [SerializeField]
    private bool viewable;
    public bool Viewable { get => viewable; private set => viewable = value; }

    [SerializeField]
    private bool smellable;
    public bool Smellable { get => smellable; private set => smellable = value; }
}
