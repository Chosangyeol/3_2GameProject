using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class C_Weapon
{
    public Enums.WeaponType weaponType;
    public float weaponDamage;
    public int weaponLevel;
    public Dictionary<int, WeaponModInstance> weaponModded = new Dictionary<int, WeaponModInstance>();

    
    public C_Weapon(Enums.WeaponType weaponType)
    {
        this.weaponType = weaponType;
        if (weaponType == Enums.WeaponType.Range)
        {
            Debug.Log("盔芭府 公扁 积己");
            weaponDamage = 5f;
            weaponLevel = 1;
            if (weaponModded.Count > 0)
                weaponModded.Clear();
        }
        else if (weaponType == Enums.WeaponType.Melee)
        {
            Debug.Log("辟芭府 公扁 积己");
            weaponDamage = 7f;
            weaponLevel = 1;
            if (weaponModded.Count > 0)
                weaponModded.Clear();
        }
    }  
}
