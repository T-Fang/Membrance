using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChangeBackState : PlayerAbilityState 
{
    public PlayerChangeBackState(Player player, PlayerFSM sm, PlayerData data, string animBoolName) : base(player, sm, data, animBoolName)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        /*
        GameObject ToBeAbsorbed = Player.Instantiate(Player.DefaultPlayer, Player.transform.position, Quaternion.identity);
        ToBeAbsorbed.SetActive(true);
        */
        Player.AbsorbedPlayer = Player.DefaultPlayer.AbilityGameObject;
        Player.Ih.ConsumeChangeBackButton();
        Player.Instantiate(Player.ChangeBackParticle, Player.transform.position + new Vector3(0.0f,1.0f,0.0f), Quaternion.Euler(0.0f, 0.0f, 0.0f));
        FromChangeBack = true;
        AbilityFinished = true;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
    }
}

