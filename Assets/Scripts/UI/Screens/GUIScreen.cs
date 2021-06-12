﻿using ScriptableObjects.Channels;
using UnityEngine;

namespace UI.Screens
{
    public abstract class GUIScreen : MonoBehaviour
    {
        [SerializeField] protected InputChannel _inputChannel;
        protected KeyCode activationKey;
        public bool IsOpen;
        
        protected virtual void Awake()
        {
            UpdateUI();
        }

        public void ToggleView()
        {
            gameObject.SetActive(!gameObject.activeSelf);
            IsOpen = gameObject.activeSelf;
        }

        public abstract KeyCode GetActivationKey();
        protected abstract void UpdateUI();
    }
}