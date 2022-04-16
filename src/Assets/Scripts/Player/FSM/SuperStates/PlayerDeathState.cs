using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathState : PlayerState
{
    public PlayerDeathState(Player player, PlayerFSM sm, PlayerData data, string animBoolName) : base(player, sm, data, animBoolName)
    {
    }


    public override void EnterState()
    {
        base.EnterState();
        Player.gameMaster.ReloadLevel();
    }


    public override void ExitState()
    {
        base.ExitState();
        Player.GetComponent<BoxCollider2D>().enabled = false;
    }


    public override void UpdateLogic()
    {
        base.UpdateLogic();
    }

    public override void HandlePhysics()
    {
        base.HandlePhysics();
    }
}
