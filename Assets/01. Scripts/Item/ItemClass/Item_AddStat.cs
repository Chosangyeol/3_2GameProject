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
        }

        public override bool OnUpdateInventory(C_Model model, float delta)
        {
            return true;
        }

        public override void OnRemoveInventory(C_Model model)
        {
            Debug.Log( data.itemName + " ªË¡¶µ ");
           
        }

        //private SPlayerStat SetPlayerStat(bool reverse = false)
        //{
        //    float value = reverse ? -data.increAmount : data.increAmount;

        //    if (data.canIncreStats == CanIncreStats.hpMax)
        //    {
        //        return new SPlayerStat { hpMax = Mathf.RoundToInt(value) };
        //    }
        //    else if (data.canIncreStats == CanIncreStats.hpRegenPerSecond)
        //    {
        //        return new SPlayerStat { hpRegenPerSecond = Mathf.RoundToInt(value) };
        //    }
        //    else if (data.canIncreStats == CanIncreStats.speedMove)
        //    {
        //        return new SPlayerStat { speedMove = value };
        //    }
        //    else if (data.canIncreStats == CanIncreStats.attackDamage)
        //    {
        //        return new SPlayerStat { attackDamage = Mathf.RoundToInt(value) };
        //    }
        //    else if (data.canIncreStats == CanIncreStats.critPercent)
        //    {
        //        value /= 100;
        //        return new SPlayerStat { critPercent = value };
        //    }
        //    else if (data.canIncreStats == CanIncreStats.critDamagePercent)
        //    {
        //        value /= 100;
        //        return new SPlayerStat { critDamagePercent = value };
        //    }
        //    else
        //    {
        //        return new SPlayerStat();
        //    }
        //}
    }
}

