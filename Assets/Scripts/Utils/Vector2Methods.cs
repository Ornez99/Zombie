using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector2Methods {

    public static Vector3 ToXZ(Vector2 vector2) {
        return new Vector3(vector2.x, 0, vector2.y);
    }

}
