﻿using Abilities;
using Abilities.Bow;
using HeroEditor.Common.Enums;
using Player;
using State.States.ArcherStates;
using UnityEngine;

namespace Entity.Player.Bow
{
    public class BowStates : WeaponStates<ShootArrowAbility>
    {
        private Player _archer;

        [Header("Shoot Fire Arrow Ability")]
        [SerializeField] private ShootArrowAbility _fireArrowAbility;

        [Header("Fast Attack Buff")]
        [SerializeField] private FastAttackBuff _fastAttackBuff;

        private ArcherShootArrowAbilityState _strongArrowState;
        private ArcherApplyFastAttackBuffState _fastAttackBuffState;

        protected override EquipmentPart WeaponEquipmentType => EquipmentPart.Bow;

        public override void Initialize()
        {
            _archer = GetComponent<Player>();
            
            BasicAttackState = new ArcherShootArrowAbilityState(_archer, _basicAttackAbility as ShootArrowAbility);
            _strongArrowState = new ArcherShootArrowAbilityState(_archer, _fireArrowAbility);
            _fastAttackBuffState =
                new ArcherApplyFastAttackBuffState(_archer, _fastAttackBuff);
        }

        public override void HookStates()
        {
            //_playerStates.AddAttackState(_strongArrowState, this);
            //_playerStates.AddBuffState(_fastAttackBuffState,() => _fastAttackBuffState.IsBuffApplied);
        }

        protected override void ActivateStates()
        {
            AnimationEvents.BowChargeEndEvent += BowChargeEndEvent;
            BasicAttackState.Ability.Activate();
            _strongArrowState.Ability.Activate();
            _fastAttackBuffState.Ability.Activate();
        }

        protected override void DeActivateStates()
        {
            BasicAttackState.Ability.Deactivate();
            _strongArrowState.Ability.Deactivate();
            _fastAttackBuffState.Ability.Deactivate();
            
            AnimationEvents.BowChargeEndEvent -= BowChargeEndEvent;
        }

        private void BowChargeEndEvent()
        {
            IsAbilityAnimationActivated = false;
            _combatChannel.OnUseAbility(_archer);
        }
    }
}