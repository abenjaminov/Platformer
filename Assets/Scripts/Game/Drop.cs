﻿using System;
using Entity;
using ScriptableObjects;
using ScriptableObjects.Channels;
using ScriptableObjects.Inventory.ItemMetas;
using State;
using State.States.DropStates;
using UnityEngine;

namespace Game
{
    public class Drop : MonoBehaviour
    {
        [SerializeField] private float _lifeSpan;
        [SerializeField] private float _dropHeight;
        [SerializeField] private LocationsChannel _locationsChannel;

        private GroundCheck _groundCheck;
        private FloatUpDown _floatComponent;
        private FadeoutToPoint _fadeoutComponent;
        private SpriteRenderer _renderer;
        private float _timeAlive;
        private BoxCollider2D _collider;

        private bool _isPickedUp;

        public bool IsPickedUp => this._isPickedUp;
        
        private StateMachine _stateMachine;
        
        private PickedUpState _pickedUpState;
        private DroppedState _droppedState;
        private FloatState _floatState;
        
        [HideInInspector] public InventoryItemMeta InventoryItemMeta { get; set; }
        [HideInInspector] public int Amount { get; private set; }

        private void Awake()
        {
            
            _renderer = GetComponentInChildren<SpriteRenderer>();
            _groundCheck = GetComponentInChildren<GroundCheck>();
            _collider = GetComponent<BoxCollider2D>();
            _fadeoutComponent = GetComponent<FadeoutToPoint>();
            _fadeoutComponent.enabled = false;
            _floatComponent = GetComponent<FloatUpDown>();
            _floatComponent.enabled = false;
            
            _stateMachine = new StateMachine();

            var dropTransform = transform;
            _droppedState = new DroppedState(_floatComponent, dropTransform, _dropHeight, _fadeoutComponent);
            _floatState = new FloatState(_floatComponent, _groundCheck, _collider, dropTransform);
            _pickedUpState = new PickedUpState(_floatComponent, _fadeoutComponent, dropTransform);

            var shouldFloat = new Func<bool>(() => _groundCheck.IsOnGround);
            var shouldBePickedUp = new Func<bool>(() => _isPickedUp);
            
            _stateMachine.AddTransition(_floatState,shouldFloat, _droppedState);
            _stateMachine.AddTransition(_pickedUpState,shouldBePickedUp, _floatState);
            
            _stateMachine.SetState(_droppedState);
            
            _locationsChannel.ChangeLocationCompleteEvent += ChangeLocationCompleteEvent;
        }

        private void OnDestroy()
        {
            _locationsChannel.ChangeLocationCompleteEvent -= ChangeLocationCompleteEvent;
        }

        private void ChangeLocationCompleteEvent(SceneMeta arg0, SceneMeta arg1)
        {
            _timeAlive += _lifeSpan;
        }

        public void SetInventoryItemMeta(InventoryItemMeta invItemMeta, int amount)
        {
            InventoryItemMeta = invItemMeta;
            _renderer.sprite = invItemMeta.ItemSprite;

            var width = invItemMeta.ItemSprite.rect.width;
            var height = invItemMeta.ItemSprite.rect.height;

            var destWidth = 100f;
            var destHeight = 100f;

            _renderer.transform.localScale = new Vector3(destWidth / width, destHeight / height, 1);
            Amount = amount;
        }

        private void Update()
        {
            _timeAlive += Time.deltaTime;

            if (_timeAlive >= _lifeSpan)
            {
                Destroy(gameObject);
            }

            _stateMachine.Tick();
        }

        public void OnPickedUp(Transform pickupTransform)
        {
            _pickedUpState.SetPickupTransform(pickupTransform);
            _isPickedUp = true;
        }
    }
}