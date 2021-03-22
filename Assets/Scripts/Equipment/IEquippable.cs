﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEquippable {

    void Equip(Unit unit);
    void Unequip(Unit unit);

}