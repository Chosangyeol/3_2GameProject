using NUnit.Framework;
using Player;
using System.Collections.Generic;
using UnityEngine;

public class C_Weapon
{
    public C_Model owner;
    public Enums.WeaponType weaponType;
    public IPlayerAttackBe attackBehavior;
    public bool isChargeWeapon;
    public float weaponDamage;
    public float weaponAttackSpeed;
    public int weaponLevel;
    public Dictionary<int, WeaponModInstance> weaponModded = new Dictionary<int, WeaponModInstance>();

    
    public C_Weapon(Enums.WeaponType weaponType)
    {
        this.weaponType = weaponType;
        if (weaponType == Enums.WeaponType.Range)
        {
            Debug.Log("盔芭府 公扁 积己");
            owner = GameObject.FindGameObjectWithTag("Player").GetComponent<C_Model>();
            attackBehavior = new BowAttackBehavior(Resources.Load<GameObject>("DefaultProjectile"));
            isChargeWeapon = true;
            weaponDamage = 5f;
            weaponLevel = 1;
            if (weaponModded.Count > 0)
                weaponModded.Clear();
        }
        else if (weaponType == Enums.WeaponType.Melee)
        {
            Debug.Log("辟芭府 公扁 积己");
            owner = GameObject.FindGameObjectWithTag("Player").GetComponent<C_Model>();
            attackBehavior = new MeleeDefaultBehavior();
            isChargeWeapon = false;
            weaponDamage = 7f;
            weaponLevel = 1;
            if (weaponModded.Count > 0)
                weaponModded.Clear();
        }
    }  
}
