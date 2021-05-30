using System.Collections.Generic;
using System.Linq;
using Platformer;
using ScriptableObjects;
using ScriptableObjects.Channels;
using TMPro;
using UnityEngine;

namespace UI
{
    public class TraitsScreen : MonoBehaviour
    {
        [SerializeField] private PlayerTraits _playerTraits;
        [SerializeField] private LevelConfiguration _levelConfiguration;
        [SerializeField] private InputChannel _inputChannel;
    
        [SerializeField] private List<GameObject> _addButtons;
    
        [SerializeField] private TextMeshProUGUI _dexText;
        [SerializeField] private TextMeshProUGUI _strText;
        [SerializeField] private TextMeshProUGUI _defText;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _pointsText;
        [SerializeField] private TextMeshProUGUI _expText;
        [SerializeField] private TextMeshProUGUI _damageText;

        private int previousExp = -1;
    
        private void Awake()
        {
            this.ToggleView();
            _inputChannel.RegisterKeyDown(KeyCode.T, ToggleView);
            UpdateTraitsUI();
        }

        private void ToggleView()
        {
            this.gameObject.SetActive(!this.gameObject.activeSelf);
        }

        private void UpdateTraitsUI()
        {
            _dexText.SetText(_playerTraits.Dexterity.ToString());
            _strText.SetText(_playerTraits.Strength.ToString());
            _defText.SetText(_playerTraits.Defense.ToString());
            _damageText.SetText(TraitsCalculator.GetMinDamage(_playerTraits) + " - " + TraitsCalculator.GetMaxDamage(_playerTraits));
        
            SetExp();

            previousExp = _playerTraits.ExperienceGained;
        
            if (_playerTraits.PointsLeft == 0)
            {
                foreach (var button in _addButtons)
                {
                    button.SetActive(false);
                }
            }
            else
            {
                foreach (var button in _addButtons)
                {
                    button.SetActive(true);
                }
            }
        }

        private void SetExp()
        {
            _levelText.SetText(_playerTraits.Level.ToString());
            _pointsText.SetText(_playerTraits.PointsLeft.ToString());
        
            if (previousExp == _playerTraits.ExperienceGained) return;
        
            var nextLevel = _levelConfiguration.Levels.FirstOrDefault(x => x.Order == _playerTraits.Level + 1);
            var expText = nextLevel != null
                ? _playerTraits.ExperienceGained.ToString() + " / " + (nextLevel.FromExp)
                : "Max Level Reached";

            _expText.SetText(expText);
        }

        private void Update()
        {
            SetExp();
        }

        public void AddStrength()
        {
            _playerTraits.PointsLeft--;
            _playerTraits.Strength++;
            UpdateTraitsUI();
        }

        public void AddDexterity()
        {
            _playerTraits.PointsLeft--;
            _playerTraits.Dexterity++;
            UpdateTraitsUI();
        }
    
        public void AddDefense()
        {
            _playerTraits.PointsLeft--;
            _playerTraits.Defense++;
            UpdateTraitsUI();
        }
    }
}
