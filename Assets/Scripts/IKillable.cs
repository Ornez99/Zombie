using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKillable {

    float MaxHealth { get; }
    float CurrentHealth { get; set; }
    float Armor { get; set; }

    void TakeDamage(float amount);
    void Heal(float amount);
}
