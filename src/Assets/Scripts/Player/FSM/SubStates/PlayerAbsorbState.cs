using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerAbsorbState : PlayerAbilityState
{
    public PlayerAbsorbState(Player player, PlayerFSM sm, PlayerData data, string animBoolName) : base(player, sm, data, animBoolName)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Player.SetVelX(0.0f);
        Player.SetVelY(0.0f);
        Player.VelocitySettable = false; // disable movements
        //Player.GetComponent<Animator>().enabled = false;
    }

    // Currently absorbed guy needs to have a 3-combo attack also, because the way attack state is setup
    // Number of attacks cannot be changed, and this will cause crash
    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if (!Player.IsAbsorbing)
        {
            // TODO: Logic for changing form, add animation for different forms
            SpriteRenderer other = Player.AbsorbedPlayer.GetComponent<SpriteRenderer>();
            Player.GetComponent<SpriteRenderer>().sprite = other.sprite;
            Player.Animator.runtimeAnimatorController = Player.AbsorbedPlayer.GetComponent<Animator>().runtimeAnimatorController;
            Player.transform.Find("Weapons/Sword/Base").GetComponent<Animator>().runtimeAnimatorController = 
                Player.AbsorbedPlayer.transform.Find("Weapons/Sword/Base").GetComponent<Animator>().runtimeAnimatorController;
            Player.transform.Find("Weapons/Sword/Weapon").GetComponent<Animator>().runtimeAnimatorController = 
                Player.AbsorbedPlayer.transform.Find("Weapons/Sword/Weapon").GetComponent<Animator>().runtimeAnimatorController;
            Player.transform.Find("Weapons/Sword/Weapon").transform.localScale = 
                Player.AbsorbedPlayer.transform.Find("Weapons/Sword/Weapon").transform.localScale;

            Player Absorbed = Player.AbsorbedPlayer.GetComponent<Player>();
            Player.SetAbsorbedId(Player.AbsorbedPlayer.GetComponent<Player>().UniqueId());
            Player.SetMagicInfo(Absorbed.data.MagicId, Absorbed.data.MagicCd, Absorbed.Magic);
            Player.magicdata = Absorbed.magicdata;
            
            Player.transform.Find("Weapons/UpSword/Base").GetComponent<Animator>().runtimeAnimatorController = 
                Player.AbsorbedPlayer.transform.Find("Weapons/UpSword/Base").GetComponent<Animator>().runtimeAnimatorController;
            Player.transform.Find("Weapons/UpSword/Weapon").GetComponent<Animator>().runtimeAnimatorController = 
                Player.AbsorbedPlayer.transform.Find("Weapons/UpSword/Weapon").GetComponent<Animator>().runtimeAnimatorController;
            Player.transform.Find("Weapons/UpSword/Weapon").transform.localScale = 
                Player.AbsorbedPlayer.transform.Find("Weapons/UpSword/Weapon").transform.localScale ;
            AbilityFinished = true;
        }
    }


    public override void ExitState()
    {
        base.ExitState();
        Player.VelocitySettable = true;
    }
}
