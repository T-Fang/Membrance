using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BigSordAbility : Ability
{
    public override void Activate(GameObject parent)
    {
        base.Activate(parent);
    }

    public void PrintSuckable()
    {
        Debug.Log("Meow");
    }
}
