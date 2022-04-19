using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSuccState : PlayerAbilityState
{
    private int _abilitiesInProximity;
    // Start is called before the first frame update
    public PlayerSuccState(Player player, PlayerFSM sm, PlayerData data, string animBoolName) : base(player, sm, data, animBoolName)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Player.Ih.ConsumeSuccButton();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        _abilitiesInProximity = Player.LootVacDetector.DetectedAbilities.Count;
        if (0 < _abilitiesInProximity)
        {
            Player.IsAbsorbing = true;
            foreach (AbilityLoot ability in Player.LootVacDetector.DetectedAbilities.ToList())
            {
                ability.StartSucking();
                // BigSordSprite needs to be a game object because can't find good sprites
                // TODO: Find good sprite which is roughly the same size as character's sprite
                Player.AbsorbedPlayer = ability.AbilityGameObject;
            }
        }

        AbilityFinished = true;
    }

    public override void ExitState()
    {
        base.ExitState();
    }
}
