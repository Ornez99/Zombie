using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSubmenu : MonoBehaviour {

    [SerializeField]
    private GameObject target;

    public GameObject Target { get => target; }

    public void ChangeActiveState() {
        Target.SetActive(!Target.activeSelf);
        Time.timeScale = (Target.activeSelf) ? 0 : 1;
    }

}
