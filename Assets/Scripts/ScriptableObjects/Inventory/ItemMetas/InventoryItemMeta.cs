﻿using UnityEngine;

namespace ScriptableObjects.Inventory.ItemMetas
{
    public abstract class InventoryItemMeta : ScriptableObject
    {
        public string Name;
        public Sprite ItemSprite;
        public Sprite InventoryItemSprite;

        public abstract void Use(Entity.Player.Player player);
    }
}