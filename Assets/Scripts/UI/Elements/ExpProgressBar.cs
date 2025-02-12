﻿using System;
using ScriptableObjects;
using ScriptableObjects.Channels;
using ScriptableObjects.GameConfiguration;
using ScriptableObjects.Traits;
using UnityEngine;

namespace UI
{
    public class ExpProgressBar : ProgressBar
    {
        [SerializeField] private PlayerTraits _playerTraits;
        [SerializeField] private PlayerChannel _playerChannel;
        [SerializeField] private LevelConfiguration _levelConfiguration;
        
        private void Awake()
        {
            UpdateMaxValue();

            CurrentValue = _playerTraits.ResistancePointsGained;
            
            _playerChannel.GainedResistancePointsEvent += GainedExperienceEvent;
            _playerChannel.LevelUpEvent += LevelUpEvent;
            
            UpdateUI();
        }

        private void OnDestroy()
        {
            _playerChannel.GainedResistancePointsEvent -= GainedExperienceEvent;
            _playerChannel.LevelUpEvent -= LevelUpEvent;
        }

        private void LevelUpEvent()
        {
            UpdateMaxValue();
            CurrentValue = _playerTraits.ResistancePointsGained;
            UpdateUI(); 
        }

        private void UpdateMaxValue()
        {
            var userLevel = _playerTraits.Level;
            if (userLevel >= _levelConfiguration.Levels.Count)
            {
                MaxValue = _levelConfiguration.Levels[_levelConfiguration.Levels.Count - 1].ExpForNextLevel;
            }
            else
            {
                MaxValue = _levelConfiguration.GetLevelByOrder(userLevel).ExpForNextLevel;
            }
        }

        private void GainedExperienceEvent(float gained)
        {
            CurrentValue = _playerTraits.ResistancePointsGained;
            UpdateUI();
        }

        protected override void UpdateUI()
        {
            var currentLevel = _playerTraits.Level;

            var expForCurrentLevel = _levelConfiguration.GetLevelByOrder(currentLevel).ExpForNextLevel;

            var expInCurrentLevel = (float)_playerTraits.ResistancePointsGained;
            
            var actualPercentage = expInCurrentLevel / expForCurrentLevel;

            var foregroundWidth = _background.rect.width * actualPercentage;
            _foreground.sizeDelta = new Vector2(foregroundWidth, _foreground.rect.height);
            
            _progressText.SetText(CurrentValue + " / " + MaxValue);
        }
    }
}