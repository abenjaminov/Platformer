﻿using Platformer.ScriptableObjects.Enums;
using UnityEngine;

namespace ScriptableObjects.Inventory
{
    [CreateAssetMenu(fileName = "Potion Item Meta", menuName = "Inventory/Potion", order = 4)]
    public class PotionItemMeta : InventoryItemMeta
    {
        public PotionType PotionType;
        public int GainAmount;

        public override void Use(Entity.Player.Player player)
        {
            if (PotionType == PotionType.Hp)
            {
                player.PlayerTraits.ChangeCurrentHealth(GainAmount);    
            }
        }
    }
}