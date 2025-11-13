using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Item
{
    [CreateAssetMenu(fileName = "AddStat Item", menuName = "SO/Data - Item/AddStat")]
    public class Item_AddStatSO : AItemDataSO
    {
        public float increAmount;
        public CanAddStats canIncreStats;

        public override AItem CreateItem()
        {
            return new Item_AddStat(this);
        }
    }

    public enum CanAddStats
    {
        hpMax = 0,
        damage,
        moveSpeed,
        attackSpeed,
        critChance,
        critDamage
    }
}

