﻿using Player;
using State.States;
using UnityEngine;

namespace State
{
    public class JumpFromLadderState : PlayerJumpingState
    {
        public JumpFromLadderState(Entity.Player.Player player, 
            PlayerMovement playerMovement,
            Rigidbody2D rigidBody2D, Collider2D _collider) : base(player, playerMovement, 0, rigidBody2D,_collider)
        {
        }
    }
}