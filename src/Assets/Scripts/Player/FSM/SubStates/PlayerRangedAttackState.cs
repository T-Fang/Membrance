using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRangedAttackState : PlayerAbilityState
{
    private float _lastMagicAttack;
    public PlayerRangedAttackState(Player player, PlayerFSM sm, PlayerData data, string animBoolName) : base(player, sm, data, animBoolName)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Player.Ih.HackyConsumption();
        Player.AudioManager.Play(Player.data.MagicId);
        Player.mentalDown(Player.data.MagicCost);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        Transform FirePoint = Player.transform.Find("Firepoint").transform;
        Player.Instantiate(Player.Magic, FirePoint.position, FirePoint.rotation);
        _lastMagicAttack = Time.unscaledTime;
        AbilityFinished = true;
    }

    public bool CanMagic()
    {
        return _lastMagicAttack + Player.data.MagicCd <= Time.unscaledTime;
    }

    public override void ExitState()
    {
        base.ExitState();
    }
}
