﻿using Entity.NPCs;
using ScriptableObjects.Chat;
using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjects.Channels
{
    [CreateAssetMenu(fileName = "Npc Channel", menuName = "Channels/Npc Channel", order = 4)]
    public class NpcChannel : ScriptableObject
    {
        public UnityAction<ChatNpc> RequestChatStartEvent;
        public UnityAction<ChatNpc, ChatSession, ChatDialogOptionAction> ChatEndedEvent;
        public UnityAction<ChatNpc, ChatSession> ChatStartedEvent;

        public void OnRequestChatStart(ChatNpc chatNpc)
        {
            RequestChatStartEvent?.Invoke(chatNpc);
        }

        public void OnChatEnded(ChatNpc chatNpc, ChatSession chatSession, ChatDialogOptionAction reason)
        {
            ChatEndedEvent?.Invoke(chatNpc, chatSession, reason);
        }

        public void OnChatStarted(ChatNpc chatNpc, ChatSession chatSession)
        {
            ChatStartedEvent?.Invoke(chatNpc, chatSession);
        }
    }
}