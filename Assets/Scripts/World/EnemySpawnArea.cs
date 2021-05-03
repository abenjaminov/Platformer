﻿using System;
using System.Collections;
using System.Collections.Generic;
using Character.Enemies;
using UnityEngine;

namespace World
{
    public class EnemySpawnArea : MonoBehaviour
    {
        [SerializeField] private List<EnemySpawnType> _enemySpawnType;
        [SerializeField] private Transform _rightBounds;
        [SerializeField] private Transform _leftBounds;
        [SerializeField] private float _spawnInterval;

        private bool _isSpawning = true;

        private List<SpawnedEnemy> _liveEnemies;

        private void Awake()
        {
            _liveEnemies = new List<SpawnedEnemy>();
            
            foreach (var enemySpawnType in _enemySpawnType)
            {
                var enemy = Instantiate(enemySpawnType.EnemyPrefab,
                    enemySpawnType.SpawnPosition.position,
                    Quaternion.identity);
                
                _liveEnemies.Add(new SpawnedEnemy()
                {
                    SpawnPosition = enemySpawnType.SpawnPosition,
                    Object = enemy,
                    Enemy = enemy.GetComponentInChildren<Enemy>()
                });
                _liveEnemies[_liveEnemies.Count - 1].Object.SetActive(false);
                var movement = enemy.GetComponent<EnemyMovement>();
                movement.LeftBounds = _leftBounds.position;
                movement.RightBounds = _rightBounds.position;
            }
        }

        private void Start()
        {
            StartCoroutine(SpawnInterval());
        }

        private IEnumerator SpawnInterval()
        {
            while (_isSpawning)
            {
                yield return new WaitForSeconds(_spawnInterval);
                
                Spawn();
            }

            yield return null;
        }

        private void Spawn()
        {
            foreach (var liveEnemy in _liveEnemies)
            {
                if (!liveEnemy.Object.activeInHierarchy)
                {
                    liveEnemy.Object.transform.position = liveEnemy.SpawnPosition.position;
                    liveEnemy.Object.SetActive(true);
                    liveEnemy.Enemy.ComeAlive();
                }
            }
        }
    }

    [Serializable]
    public class EnemySpawnType
    {
        public GameObject EnemyPrefab;
        public Transform SpawnPosition;
    }

    public struct SpawnedEnemy
    {
        public GameObject Object;
        public Enemy Enemy;
        public Transform SpawnPosition;
    }
}