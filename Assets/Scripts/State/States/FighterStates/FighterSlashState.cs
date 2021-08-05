﻿using Abilities.Fighter;
using Entity;
using Entity.Player.FighterClass;

namespace State.States.FighterStates
{
    public class FighterSlashState : PlayerAbilityState<SlashAbility>
    {
        private Entity.Player.Player _fighter;
        
        public FighterSlashState(CharacterWrapper character, SlashAbility ability) : 
            base(character,ability)
        {
            _fighter = character as Entity.Player.Player;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Ability.Use();
        }
    }
}