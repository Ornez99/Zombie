using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKillable {

    float MaxHealth { get; }
    float CurrentHealth { get; }
    float Armor { get; }

}
