﻿using System.Linq;
using Entity;
using Entity.Player.ArcherClass;
using Helpers;
using Platformer;
using ScriptableObjects.Traits;
using State;
using State.States.ArcherStates;
using UnityEngine;

namespace Abilities.Archer
{
    public class ShootArrowAbility : Attack
    {
        [SerializeField] private GameObject _arrowPrefab;
        [SerializeField] private Transform _fireTransform;

        public override void Use()
        {
            var arrow = Object.Instantiate(_arrowPrefab, _fireTransform).GetComponent<Arrow>();
            
            arrow.WorldMovementDirection = _host.GetWorldMovementDirection();
            arrow.ParentCharacter = (Entity.Player.Player) _hostWrapper;
            arrow.Range = TraitsHelper.CalculateAttackRange(_host.Traits);
            arrow.SetParentAbility(this);
            
            var sr = arrow.GetComponent<SpriteRenderer>();

            var arrowTransform = arrow.transform;
            
            arrowTransform.localPosition = Vector3.zero;
            arrowTransform.localRotation = Quaternion.identity;
            arrow.transform.SetParent(null);
            sr.sprite = GetArrowSprite();
        }

        private Sprite GetArrowSprite()
        {
            return _hostWrapper.GetCharacter().Bow.Single(j => j.name == "Arrow");
        }
    }
}