using NUnit.Framework;
using System;
using System.Collections;
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

        public bool hasSkillItem = false;

        public bool isSkill3Cool = false;

        public AItem skillItem;
        public SkillSO itemSkillSO;

        public C_Inventory(C_Model model)
        {
            this._model = model;
            items = new List<AItem>();
            return;
        }

        public void AddItem(AItem item)
        {
            ActionBeforeAddItem?.Invoke();
            CheckSkillItem(item);
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

        public void CheckSkillItem(AItem item)
        {
            if (item is Item_AddSkill)
            {
                foreach (AItem aitem in items)
                {
                    if (aitem is Item_AddSkill)
                    {
                        RemoveItem(aitem);
                        break;
                    }
                }
            }
        }

        public void UseSkill3()
        {
            if (!hasSkillItem) return;
            if (itemSkillSO == null) return;

            _model.StartCoroutine(Skill3Cooldown(5f));
            itemSkillSO.ActiveSkill();
        }

        IEnumerator Skill3Cooldown(float cool)
        {
            isSkill3Cool = true;
            Debug.Log("Skill 3 used, cooldown started.");
            yield return new WaitForSeconds(cool);
            Debug.Log("Skill 3 used, cooldown End.");
            isSkill3Cool = false;
        }
    }
}

