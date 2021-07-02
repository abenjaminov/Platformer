﻿using System;
using System.Collections.Generic;
using System.Linq;
using Assets.HeroEditor.Common.CommonScripts;
using ScriptableObjects.Channels;
using ScriptableObjects.Chat;
using TMPro;
using UI.Screens;
using UnityEngine;

namespace Platformer.UI
{
    public class ScreensManager : MonoBehaviour
    {
        [SerializeField] private InputChannel _inputChannel;
        [SerializeField] private NpcChannel _NpcChannel;
        [SerializeField] private GUIScreen _traitsScreen;
        [SerializeField] private GUIScreen _inventory;
        
        [SerializeField] private ChatScreen _chatScreen;

        private Stack<GUIScreen> _openScreens;

        private void Awake()
        {
            _openScreens = new Stack<GUIScreen>();
            _inputChannel.SubscribeKeyDown(_traitsScreen.GetActivationKey(), ToggleTraitsScreen);
            _inputChannel.SubscribeKeyDown(_inventory.GetActivationKey(), ToggleInventoryScreen);
            
            _inputChannel.SubscribeKeyDown(KeyCode.Escape, ClosePrevScreen);
            
            _NpcChannel.RequestChatStartEvent += RequestChatStartEvent;
        }

        private void ToggleTraitsScreen()
        {
            ToggleScreen(_traitsScreen);
        }
        
        private void ToggleInventoryScreen()
        {
            ToggleScreen(_inventory);
        }
        
        private void OnDestroy()
        {
            //_inputChannel.UnRegisterKeyDown(_traitsScreen.GetActivationKey(), ToggleTraitsScreen);
            //_inputChannel.UnRegisterKeyDown(_inventory.GetActivationKey(), ToggleInventoryScreen);
            //_inputChannel.UnRegisterKeyDown(KeyCode.Escape, ClosePrevScreen);
        }

        private void RequestChatStartEvent(ChatSession arg0)
        {
            if (!_chatScreen.isActiveAndEnabled)
            {
                _chatScreen.CurrentChatSession = arg0;
                _chatScreen.SetActive(true);
                _chatScreen.StartChat();
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
            if (_openScreens.Count == 0) return;
            
            var prevScreen = _openScreens.Pop();
            
            ToggleScreen(prevScreen);
        }
    }
}