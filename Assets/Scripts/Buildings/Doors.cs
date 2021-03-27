using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : Building, IInteractable {

    [SerializeField]
    private bool opened;
    [SerializeField]
    private float openingTimer;
    [SerializeField]
    private float openingTime = 0f;
    [SerializeField]
    private Animator animator = null;
    [SerializeField]
    private GameObject highlight = null;

    public bool Enabled { get => openingTimer <= 0f; }

    private void Update() {
        if (openingTimer > 0f) {
            openingTimer -= Time.deltaTime;
            if (openingTimer <= 0f) {
                opened = !opened;
            }
        }
    }

    public void Highlight() {
        highlight.SetActive(true);
    }

    public void Interact(Unit unit) {
        if (openingTimer <= 0f) {
            openingTimer = openingTime;
            animator.SetBool("Opened", !opened);

            Viewable = !Viewable;
        }
    }

    public void StopHighlight() {
        highlight.SetActive(false);
    }
}
