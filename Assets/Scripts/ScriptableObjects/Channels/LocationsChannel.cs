﻿using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjects.Channels
{
    [CreateAssetMenu(fileName = "Locations Channel", menuName = "Channels/Locations Channel", order = 5)]
    public class LocationsChannel : ScriptableObject
    {
        public UnityAction<SceneMeta, SceneMeta> ChangeLocationEvent;
        public UnityAction<SceneMeta, SceneMeta> ChangeLocationCompleteEvent;
        public UnityAction<SceneMeta, SceneMeta> RespawnEvent;

        public SceneMeta CurrentScene;

        public bool IsChangingLocation;

        public void OnChangeLocation(SceneMeta destination, SceneMeta source)
        {
            IsChangingLocation = true;
            ChangeLocationEvent?.Invoke(destination, source);
        }

        public void OnRespawn(SceneMeta destination, SceneMeta source)
        {
            RespawnEvent?.Invoke(destination, source);
        }

        public void OnChangeLocationComplete(SceneMeta destination, SceneMeta source)
        {
            IsChangingLocation = false;
            CurrentScene = destination;
            ChangeLocationCompleteEvent?.Invoke(destination, source);
        }
    }
}