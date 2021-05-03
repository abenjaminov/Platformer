﻿using System;
using ScriptableObjects.Channels;
using State;
using State.States;
using State.States.EnemyStates;
using UnityEngine;

namespace Character.Enemies
{
    public class EnemyStates : MonoBehaviour
    {
        private StateMachine _stateMachine;

        [SerializeField] private CombatChannel _combatChannel;
        [SerializeField] private float _walkingSpeed;
        [SerializeField] private float _idleTimeBetweenTargets;
        [SerializeField] private float _deadDelayTime;
        
        private IState _defaultState;

        private Enemy _enemy;

        private void Awake()
        {
            _stateMachine = new StateMachine(false);
            
            var enemyMovement = GetComponentInParent<EnemyMovement>();
            var animator = GetComponent<Animator>();
            _enemy = GetComponent<Enemy>();
            
            var idle = new IdleState(enemyMovement);
            var walk = new EnemyWalkState(enemyMovement, animator, _walkingSpeed);
            var dead = new EnemyDieState(enemyMovement,animator,_combatChannel, _enemy);
            var vanishState = new EmptyState();

            var ShouldStand = new Func<bool>(() => !_enemy.IsDead && enemyMovement.Target != Vector2.positiveInfinity &&
                                                   Vector2.Distance(enemyMovement.Target, enemyMovement.transform.position) <= .1f);
            var ShouldWalk = new Func<bool>(() => !_enemy.IsDead && idle.IdleTime >= _idleTimeBetweenTargets);
            var ShouldDie = new Func<bool>(() => _enemy.IsDead);
            var ShouldVanish = new Func<bool>(() => _enemy.IsDead && dead.TimeDead >= _deadDelayTime);
            
            _stateMachine.AddTransition(idle, ShouldStand, walk);
            _stateMachine.AddTransition(walk, ShouldWalk, idle);
            
            _stateMachine.AddTransition(dead, ShouldDie, idle);
            _stateMachine.AddTransition(dead, ShouldDie, walk);
            
            _stateMachine.AddTransition(vanishState,ShouldVanish,null, () =>
            {
                enemyMovement.gameObject.SetActive(false);
            });

            _defaultState = idle;
        }

        private void OnEnable()
        {
            _stateMachine.SetState(_defaultState);
        }

        private void Start()
        {
            _stateMachine.SetState(_defaultState);
        }

        private void Update()
        {
            _stateMachine.Tick();
        }
    }
}