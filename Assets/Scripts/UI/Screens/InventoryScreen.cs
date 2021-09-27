using System.Collections.Generic;
using System.Linq;
using Helpers;
using ScriptableObjects.Channels;
using ScriptableObjects.Inventory;
using ScriptableObjects.Inventory.ItemMetas;
using TMPro;
using UnityEngine;

namespace UI.Screens
{
    public class InventoryScreen : GUIScreen
    {
        private Entity.Player.Player _player;
        [SerializeField] private Inventory _inventory;
        [SerializeField] private InventoryChannel _invChannel;
        [SerializeField] private EquipmentDetailsPanel _equipmentDetailsPanel;

        [SerializeField] private TextMeshProUGUI _coinText;
        private List<InventoryItemView> _itemViews = new List<InventoryItemView>();

        protected override void Awake()
        {
            _itemViews = GetComponentsInChildren<InventoryItemView>().ToList();
            
            foreach (var itemView in _itemViews)
            {
                itemView.ItemViewDoubleClicked += ItemViewDoubleClicked;
                itemView.ItemViewMouseEnter += ItemViewMouseEnter;
                itemView.ItemViewMouseExit += ItemViewMouseExit;
            }
            
            base.Awake();
        }

        public void ItemViewMouseExit(EquipmentItemView item)
        {
            _equipmentDetailsPanel.HideItemDetails();
        }

        public void ItemViewMouseEnter(EquipmentItemView itemView)
        {
            if (itemView.ItemMeta == null) return;
            
            _equipmentDetailsPanel.ShowItemDetails(itemView.ItemMeta, itemView.GetBottomLeftCorner());
        }

        void Start()
        {
            _player = FindObjectOfType<Entity.Player.Player>();
            _invChannel.ItemAddedSilentEvent += ItemAddedSilentEvent;
        }

        private void ItemViewDoubleClicked(EquipmentItemView item)
        {
            OnItemDoubleClicked(_itemViews.IndexOf(item as InventoryItemView));
        }

        private void OnDestroy()
        {
            _invChannel.ItemAddedEvent -= ItemAddedSilentEvent;
            foreach (var itemView in _itemViews)
            {
                itemView.ItemViewDoubleClicked -= ItemViewDoubleClicked;
                itemView.ItemViewMouseEnter -= ItemViewMouseEnter;
                itemView.ItemViewMouseExit -= ItemViewMouseExit;
            }
        }

        public override KeyCode GetActivationKey()
        {
            return KeyCode.I;
        }

        protected override void UpdateUI()
        {
            _coinText.SetText(StringHelper.NumberToString(_inventory.CurrencyItem.Amount));
            
            for (int i = 0; i < _itemViews.Count; i++)
            {
                var color = _itemViews[i].ItemSprite.color;
                    
                if (_inventory.OwnedItems.Count > i)
                {
                    _itemViews[i].ItemSprite.sprite = _inventory.OwnedItems[i].ItemMeta.InventoryItemSprite;
                    if (_inventory.OwnedItems[i].ItemMeta is EquipmentItemMeta)
                    {
                        _itemViews[i].AmountText.SetText("");
                    }
                    else
                    {
                        _itemViews[i].AmountText.SetText(_inventory.OwnedItems[i].Amount.ToString());    
                    }
                    
                    _itemViews[i].ItemSprite.color = new Color(color.r, color.g, color.b, 255);
                    _itemViews[i].ItemMeta = _inventory.OwnedItems[i].ItemMeta;
                }
                else
                {
                    _itemViews[i].ItemSprite.sprite = null;
                    _itemViews[i].ItemSprite.color = new Color(color.r, color.g, color.b, 0);
                    _itemViews[i].AmountText.SetText("");
                    _itemViews[i].ItemMeta = null;
                }
            }
        }

        private void ItemAddedSilentEvent(InventoryItem item, int amountAdded)
        {
            UpdateUI();
        }

        private void OnItemDoubleClicked(int itemIndex)
        {
            if (itemIndex >= _inventory.OwnedItems.Count) return;
            
            _inventory.UseItem(_player, _inventory.OwnedItems[itemIndex]);
            UpdateUI();
        }
    }
}
