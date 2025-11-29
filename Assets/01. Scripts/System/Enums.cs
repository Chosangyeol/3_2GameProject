using UnityEngine;

public class Enums : MonoBehaviour
{
    public enum CharacterStat
    {
        Hp,
        Damage,
        MoveSpeed,
        AttackSpeed,
        CriticlaRate,
        CriticalDamage
    }

    public enum WeaponType
    {
        None,
        Bow,
        Staff
    }

    public enum WeaponModType
    {
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
