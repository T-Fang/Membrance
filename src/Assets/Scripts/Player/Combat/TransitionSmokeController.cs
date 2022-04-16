using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionSmokeController : MonoBehaviour
{
    private AbilityLoot _loot;

    public void Init(AbilityLoot loot)
    {
        _loot = loot;
    }

    public void FinishAnimation()
    {
        // TODO: Find alternative..
        Player.SetFinishAbsorbing();
        Destroy(gameObject);
    }

}
