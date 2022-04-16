using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.TextCore;
using Vector2 = UnityEngine.Vector2;

public class PlayerDashState : PlayerAbilityState
{
    public bool CanDash { get; private set; }
    private float _lastDashTime;
    private Vector2 _dashDir;
    private Vector2 _dashDirInput;
    private Vector2 _lastAfterImagePos;
    public PlayerDashState(Player player, PlayerFSM sm, PlayerData data, string animBoolName) : base(player, sm, data, animBoolName)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Player.AudioManager.Play("Dash");

        CanDash = false;
        Player.Ih.ConsumeDashButton();

        _dashDir = Vector2.right * Player.FacingDir;
    }

    public override void ExitState()
    {
        base.ExitState();

        // Damp only when we dashing any direction other than down
        if (0.001f < Player.CurrVel.y) Player.SetVelY(Player.CurrVel.y * Data.dashEndDamper);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        //_dashDirInput = Player.Ih.DashDirInput;
        _dashDirInput = Player.Ih.RawInput;
        if (_dashDirInput != Vector2.zero)
        {
            _dashDir = _dashDirInput;
            _dashDir.Normalize();
            var angle = Vector2.SignedAngle(Vector2.right, _dashDir);
            Player.FlipIfNeeded(Mathf.RoundToInt(_dashDir.x));
            Player.BodyRef.drag = Data.dragForce;
            Player.SetVelAtAngle(Data.dashVel, _dashDir);
            PlaceAfterImageIfNeeded();

            // If haven't dash finish, exit function before setting dash as "finished"
            if (!(StartTime + Data.dashTime <= Time.unscaledTime)) return;
            Player.BodyRef.drag = 0.0f;
            AbilityFinished = true;
            _lastDashTime = Time.unscaledTime;
        }
        else
        {
            Player.BodyRef.drag = 0.0f;
            AbilityFinished = true;
            _lastDashTime = Time.unscaledTime;
        }
    }

    public bool PlayerCanDash()
    {
        return CanDash && _lastDashTime + Data.dashCD <= Time.unscaledTime && Player.DashHolder.IsLearned();
    }

    public void ResetDashStamina() => CanDash = true;

    private void GrabAndInstallAfterImage()
    {
        AfterImagePool.SingleTonInstance.grab_from_pool();
        _lastAfterImagePos = Player.transform.position;
    }

    private void PlaceAfterImageIfNeeded()
    {
        if (Data.afterImageDist <= Vector2.Distance(Player.transform.position, _lastAfterImagePos))
        {
            GrabAndInstallAfterImage();
        }
    }

}
