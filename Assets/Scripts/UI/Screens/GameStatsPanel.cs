using ScriptableObjects;
using TMPro;
using UnityEngine;

public class GameStatsPanel : MonoBehaviour
{
    [SerializeField] private PlayerTraits _playerTraits;
    [SerializeField] private TextMeshProUGUI _levelText;
    
    void Awake()
    {
        UpdateLevelText();
        _playerTraits.LevelUpEvent += LevelUpEvent;
    }

    private void UpdateLevelText()
    {
        _levelText.SetText("Level " + _playerTraits.Level);
    }

    private void LevelUpEvent()
    {
        UpdateLevelText();
    }
}