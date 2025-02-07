﻿using Character;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace Entity.Enemies
{
    public class EnemyMovement : MonoBehaviour, ICharacterMovement
    {
        [HideInInspector] public Vector2 Target = Vector2.positiveInfinity;
        private float _horizontalSpeed;
        private Vector2 _direction;
        [SerializeField] private GameObject _enemyVisuals;

        [HideInInspector] public Vector2 LeftBounds;
        [HideInInspector] public Vector2 RightBounds;
        
        private void Update()
        {
            transform.Translate(_direction*(_horizontalSpeed * Time.deltaTime));
        }

        public void SetTarget(Vector2 target)
        {
            if (target.x < transform.position.x)
            {
                _direction = Vector2.left;
                SetYRotation(180);
            }
            else
            {
                _direction = Vector2.right;
                SetYRotation(0);
            }

            Target = target;
        }
        
        public void SetHorizontalVelocity(float horizontalVelocity)
        {
            _horizontalSpeed = horizontalVelocity;
        }

        public void SetVelocity(Vector2 velocity)
        {
            _horizontalSpeed = velocity.x;
        }

        public void SetYRotation(float yRotation)
        {
            _enemyVisuals.transform.rotation = Quaternion.Euler(0, yRotation, 0);
        }
    }
}