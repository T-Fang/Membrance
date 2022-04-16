using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface KnockbackableObject
{
    void Knockback(Vector2 angle, float magnitude, int direciton);
}
