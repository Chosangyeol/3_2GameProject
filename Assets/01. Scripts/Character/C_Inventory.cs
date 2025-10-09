using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Item
{
    [Serializable]
    public class C_Inventory
    {
        private readonly C_Model _model;
        private List<AItem> items;

        public event Action ActionBeforeAddItem;
        public event Action ActionAfterAddItem;
        public event Action ActionBeforeRemoveItem;
        public event Action ActionAfterRemoveItem;

        public C_Inventory(C_Model model)
        {
            this._model = model;
            items = new List<AItem>();
            return;
        }

        public void AddItem(AItem item)
        {
            ActionBeforeAddItem?.Invoke();
            item.OnAddInventory(_model);
            items.Add(item);
            ActionAfterAddItem?.Invoke();
            return;
        }

        public void UpdateItem(float delta)
        {
            for (int i = 0; i < items.Count; i++)
            {
                AItem item = items[i];

                item.OnUpdateInventory(_model, delta);
            }
            return;
        }

        public bool RemoveItem(AItem item)
        {
            if (items.Contains(item))
            {
                ActionBeforeRemoveItem?.Invoke();
                items.Remove(item);
                ActionAfterRemoveItem?.Invoke();
                return (true);
            }
            return (false);
        }
    }
}

