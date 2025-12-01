using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Item
{
    public class Item_AddStat : AItem
    {
        private Item_AddStatSO data;

        public Item_AddStat(Item_AddStatSO itemData) : base(itemData)
        {
            data = itemData;
        }

        public override void OnAddInventory(C_Model model)
        {
            Debug.Log(data.itemName + " √ﬂ∞°µ ");
            AddStat(model,data.increAmount);
        }

        public override bool OnUpdateInventory(C_Model model, float delta)
        {
            return true;
        }

        public override void OnRemoveInventory(C_Model model)
        {
            Debug.Log( data.itemName + " ªË¡¶µ ");
            RemoveStat(model,data.increAmount);
        }

        private void AddStat(C_Model model,float amount)
        {
            switch (data.canIncreStats)
            {
                case CanAddStats.hpMax:
                    model.GetStat().AddMaxHp(Mathf.RoundToInt(amount));
                    break;
                case CanAddStats.damage:
                    model.GetStat().AddDamage(Mathf.RoundToInt(amount));
                    break;
                case CanAddStats.moveSpeed:
                    model.GetStat().AddMoveSpeed(amount);
                    break;
                case CanAddStats.critChance:
                    model.GetStat().AddCritRate(amount);
                    break;
                case CanAddStats.critDamage:
                    model.GetStat().AddCritDamage(amount);
                    break;
            }
        }

        private void RemoveStat(C_Model model, float amount)
        {
            switch (data.canIncreStats)
            {
                case CanAddStats.hpMax:
                    model.GetStat().AddMaxHp(-Mathf.RoundToInt(amount));
                    break;
                case CanAddStats.damage:
                    model.GetStat().AddDamage(-Mathf.RoundToInt(amount));
                    break;
                case CanAddStats.moveSpeed:
                    model.GetStat().AddMoveSpeed(-amount);
                    break;
                case CanAddStats.critChance:
                    model.GetStat().AddCritRate(-amount);
                    break;
                case CanAddStats.critDamage:
                    model.GetStat().AddCritDamage(-amount);
                    break;
            }
        }
    }
}

