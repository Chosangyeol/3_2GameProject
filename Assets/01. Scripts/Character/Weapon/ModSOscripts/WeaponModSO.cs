using JetBrains.Annotations;
using NUnit.Framework;
using System.Collections.Generic;
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
    public string modName;
    public string modDesc;
    public int modGrade;
    public int maxLevel = 2;

    public virtual void ApplyMod(C_Weapon weapon, C_StatBase owner, int level) { }
    public virtual void OnFire(C_Weapon weapon, ref List<GameObject> projectiless, float speed) { }

}
