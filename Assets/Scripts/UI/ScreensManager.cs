﻿using System.Collections.Generic;
using System.Linq;
using Assets.HeroEditor.Common.CommonScripts;
using Entity.NPCs;
using ScriptableObjects.Channels;
using ScriptableObjects.GameConfiguration;
using ScriptableObjects.Quests;
using UI.Screens;
using UnityEngine;

namespace UI
{
    public class ScreensManager : MonoBehaviour
    {
        [Header("Channels")]
        [SerializeField] private PersistenceChannel _persistenceChannel;
        [SerializeField] private InputChannel _inputChannel;
        [SerializeField] private NpcChannel _NpcChannel;
        [SerializeField] private QuestsChannel _questsChannel;
        [SerializeField] private QuestsList _questsList;
        [SerializeField] private GameChannel _gameChannel;

        [Header("Screens")]
        [SerializeField] private GUIScreen _traitsScreen;
        [SerializeField] private GUIScreen _inventory;
        [SerializeField] private GUIScreen _equipment;
        [SerializeField] private GUIScreen _map;
        [SerializeField] private GUIScreen _settings;
        
        [SerializeField] private ChatSelectionScreen _chatSelectionScreen;
        [SerializeField] private ChatScreen _chatScreen;
        [SerializeField] private QuestTrackerPanel _questTrackerPanel;
        
        [SerializeField] private List<GameObject> _defaultViews;

        private List<KeySubscription> _subscriptions = new List<KeySubscription>();
        
        private Stack<GUIScreen> _openScreens;

        private string _currentCloseGameMessageId;

        private void Awake()
        {
            _persistenceChannel.GameModulesLoadedEvent += GameModulesLoadedEvent;
            _openScreens = new Stack<GUIScreen>();
            
            _NpcChannel.RequestChatStartEvent += RequestChatStartEvent;
            _gameChannel.MessageClosedEvent += MessageClosedEvent;
        }

        private void MessageClosedEvent(string messageId, MessageOptions response)
        {
            if (messageId != _currentCloseGameMessageId) return;
            
            if (response == MessageOptions.Yes)
            {
                Application.Quit();
            }
        }

        private void GameModulesLoadedEvent()
        {
            _subscriptions.Add(_inputChannel.SubscribeKeyDown(_traitsScreen.GetActivationKey(), ToggleTraitsScreen));
            _subscriptions.Add(_inputChannel.SubscribeKeyDown(_inventory.GetActivationKey(), ToggleInventoryScreen));
            _subscriptions.Add(_inputChannel.SubscribeKeyDown(_map.GetActivationKey(), ToggleMapScreen));
            _subscriptions.Add(_inputChannel.SubscribeKeyDown(_equipment.GetActivationKey(), ToggleEquipmentScreen));

            _subscriptions.Add(_inputChannel.SubscribeKeyDown(KeyCode.Escape, ClosePrevScreen));

            foreach (var gameObject in _defaultViews)
            {
                gameObject.SetActive(true);
            }
        }

        private void ToggleEquipmentScreen()
        {
            ToggleScreen(_equipment);
        }
        
        private void ToggleSettingsScreen()
        {
            ToggleScreen(_settings);
        }
        
        private void ToggleTraitsScreen()
        {
            ToggleScreen(_traitsScreen);
        }
        
        private void ToggleInventoryScreen()
        {
            ToggleScreen(_inventory);
        }
        
        private void ToggleMapScreen()
        {
            ToggleScreen(_map);
        }
        
        private void OnDestroy()
        {
            _persistenceChannel.GameModulesLoadedEvent -= GameModulesLoadedEvent;
            _NpcChannel.RequestChatStartEvent -= RequestChatStartEvent;
            _gameChannel.MessageClosedEvent -= MessageClosedEvent;
            
            foreach (var subscription in _subscriptions)
            {
                subscription.Unsubscribe();
            }
            
            _subscriptions.Clear();
        }

        private void RequestChatStartEvent(ChatNpc chatNpc)
        {
            if (!_chatScreen.isActiveAndEnabled)
            {
                var availableSessions =
                    chatNpc.GetAvailableChatSessions();

                if (availableSessions.Count <= 0) return;
                
                _chatSelectionScreen.SetActive(true);
                _chatSelectionScreen.SelectChat(chatNpc, availableSessions);
            }
        }

        private void ToggleScreen(GUIScreen screenToToggle)
        {
            if (screenToToggle.IsOpen)
            {
                var openScreensLeft = _openScreens.Where(x => x != screenToToggle).ToList();
                _openScreens.Clear();
                foreach (var screen in openScreensLeft)
                {
                    _openScreens.Push(screen);
                }
            }
            else
            {
                _openScreens.Push(screenToToggle);
            }
            
            screenToToggle.ToggleView();    
        }

        private void ClosePrevScreen()
        {
            if (_openScreens.Count == 0)
            {
                _currentCloseGameMessageId = _gameChannel.ShowGameMessage("Are you sure you want to quit the game?", 
                    new List<MessageOptions>()
                {
                    MessageOptions.Yes, MessageOptions.No
                });
            }
            else
            {
                var prevScreen = _openScreens.Pop();
            
                ToggleScreen(prevScreen);    
            }
        }
    }
}