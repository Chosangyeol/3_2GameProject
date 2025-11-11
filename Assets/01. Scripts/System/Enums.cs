using UnityEngine;

public class Enums : MonoBehaviour
{
    public enum WeaponType
    {
        None,
        Melee,
        Range
    }

    public enum AllWeaponType
    {
        Bow,
        Staff,
        CrossBow,
        Blade,
        GiantBlade,
        Spear
    }

    public enum WeaponModType
    {
        MeleeMod,
        RangeMod
    }    

    public enum EnemyType
    {
        Melee,
        Ranged
    }

    public enum ItemRarity
    {
        Common,
        Rare,
        Unique,
        Legend
    }
}
