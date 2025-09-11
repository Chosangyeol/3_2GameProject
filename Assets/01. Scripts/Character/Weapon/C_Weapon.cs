using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class C_Weapon
{
    public Enums.WeaponType weaponType;
    public float weaponDamage;
    public int weaponLevel;
    public Dictionary<int,WeaponModSO> weaponModded = new Dictionary<int, WeaponModSO>();

    public C_Weapon(Enums.WeaponType weaponType)
    {
        this.weaponType = weaponType;
        if (weaponType == Enums.WeaponType.Range)
        {
            Debug.Log("원거리 무기 생성");
            weaponDamage = 5f;
            weaponLevel = 1;
            if (weaponModded.Count > 0)
                weaponModded.Clear();
        }
        else if (weaponType == Enums.WeaponType.Melee)
        {
            Debug.Log("근거리 무기 생성");
            weaponDamage = 7f;
            weaponLevel = 1;
            if (weaponModded.Count > 0)
                weaponModded.Clear();
        }
    }

    public void WeaponModing(int modGrade, WeaponModSO weaponMod)
    {
       if (weaponMod == null)
       {
           Debug.LogWarning("모드 데이터가 존재하지 않습니다. weaponMod is null");
           return;
       }

       if (modGrade < 1 ||  modGrade > 4)
       {
           Debug.LogWarning("모드 단계가 올바르지 않습니다. modGrade < 1 or modGrade > 4");
           return;
       }

        if (weaponModded.ContainsKey(modGrade))
        {
            Debug.LogWarning("이미 같은단계의 모드가 개조되어 있습니다. weaponMod already modded.");
            return;
        }

        weaponModded.Add(modGrade, weaponMod);

    }

    public void ResetModing()
    {
        weaponModded.Clear();
    }
}
