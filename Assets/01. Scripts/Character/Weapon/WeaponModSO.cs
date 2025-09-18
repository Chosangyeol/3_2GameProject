using JetBrains.Annotations;
using UnityEngine;

[System.Serializable]
public class WeaponModInstance
{
    public WeaponModSO weaponMod;
    public int level;
}

[CreateAssetMenu(fileName = "New WeaponMod", menuName = "WeaponMod")]
public class WeaponModSO : ScriptableObject
{
    public Enums.WeaponModType modType;
    public Enums.WeaponType allowedWeaponType;
    public int maxLevel = 2;

    [Header("���ݷ� ����")]
    public float addDamageLv1;
    public float addDamageLv2;

    [Header("���ݼӵ� ����")]
    public float addAttackSpeedLv1;
    public float addAttackSpeedLv2;
}
