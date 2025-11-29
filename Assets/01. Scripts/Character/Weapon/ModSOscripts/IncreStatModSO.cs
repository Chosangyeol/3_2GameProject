using UnityEngine;

[CreateAssetMenu(fileName = "New WeaponMod", menuName = "WeaponMod/IncreStatMod")]
public class IncreStatModSO : WeaponModSO
{
    [Header("증가 스탯 종류")]
    public Enums.CharacterStat statType;
    [Header("증가량")]
    public float amount;

    public override void ActivateMod(C_Weapon weapon, C_StatBase owner)
    {
        switch (statType)
        {
            case Enums.CharacterStat.Damage:
                weapon.modDamage += Mathf.RoundToInt(amount);
                break;
            case Enums.CharacterStat.AttackSpeed:
                weapon.modAttackSpeed -= amount;
                break;
            case Enums.CharacterStat.CriticlaRate:
                weapon.modCritRate += amount;
                break;
            case Enums.CharacterStat.CriticalDamage:
                weapon.modCritDamage += amount;
                break;
        }
    }

    public override void DeactivateMod(C_Weapon weapon, C_StatBase owner)
    {
        switch (statType)
        {
            case Enums.CharacterStat.Damage:
                weapon.modDamage -= Mathf.RoundToInt(amount);
                break;
            case Enums.CharacterStat.AttackSpeed:
                weapon.modAttackSpeed += amount;
                break;
            case Enums.CharacterStat.CriticlaRate:
                weapon.modCritRate -= amount;
                break;
            case Enums.CharacterStat.CriticalDamage:
                weapon.modCritDamage -= amount;
                break;
        }
    }
}
