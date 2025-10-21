using UnityEngine;

[CreateAssetMenu(fileName = "New WeaponMod", menuName = "WeaponMod/ChangeWeapon")]
public class WeaponChangeModSO : WeaponModSO
{
    public Enums.AllWeaponType changeType;

    public override void ApplyMod(C_Weapon weapon, C_StatBase owner, int level)
    {
        base.ApplyMod(weapon, owner, level);
        if (changeType == Enums.AllWeaponType.Staff)
            weapon.attackBehavior = new StaffAttackBehavior(Resources.Load<GameObject>("DefaultProjectile"));
        else if (changeType == Enums.AllWeaponType.CrossBow)
            weapon.attackBehavior = new CrossBowAttackBehavior(Resources.Load<GameObject>("DefaultProjectile"));
        else if (changeType == Enums.AllWeaponType.Bow)
            weapon.attackBehavior = new BowAttackBehavior(Resources.Load<GameObject>("DefaultProjectile"));
        else if (changeType == Enums.AllWeaponType.Blade)
            Debug.Log("검");
        else if (changeType == Enums.AllWeaponType.GiantBlade)
            Debug.Log("대검");
        else if (changeType == Enums.AllWeaponType.Spear)
            Debug.Log("창");

        Debug.Log(weapon.weaponType);
    }
}
