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

    public int baseWeaponDamage;
    public int modDamage;
    public int totalWeaponDamage;

    public float baseWeaponAttackSpeed;
    public float modAttackSpeed;
    public float totalWeaponAttackSpeed;

    public float baseCritRate;
    public float modCritRate;
    public float totalCritRate;

    public float baseCritDamage;
    public float modCritDamage;
    public float totalCritDamage;

    public int weaponLevel;
    public Dictionary<int, WeaponModInstance> weaponModded = new Dictionary<int, WeaponModInstance>();
    public Dictionary<int, SkillSO> weaponSkills = new Dictionary<int, SkillSO>();


    
    public C_Weapon(Enums.WeaponType weaponType)
    {
        this.weaponType = weaponType;
        if (weaponType == Enums.WeaponType.Bow)
        {
            Debug.Log("활 무기 생성");
            owner = GameObject.FindGameObjectWithTag("Player").GetComponent<C_Model>();
            attackBehavior = new BowAttackBehavior(Resources.Load<GameObject>("DefaultArrow"));
            isChargeWeapon = true;
            baseWeaponDamage = 5;
            baseWeaponAttackSpeed = 1.0f;
            weaponLevel = 1;
            if (weaponModded.Count > 0)
                weaponModded.Clear();

        }
        else if (weaponType == Enums.WeaponType.Staff)
        {
            Debug.Log("스태프 무기 생성");
            owner = GameObject.FindGameObjectWithTag("Player").GetComponent<C_Model>();
            attackBehavior = new StaffAttackBehavior(Resources.Load<GameObject>("DefaultFireBall"));
            isChargeWeapon = true;
            baseWeaponDamage = 6;
            baseWeaponAttackSpeed = 1.5f;
            weaponLevel = 1;
            if (weaponModded.Count > 0)
                weaponModded.Clear();
        }
    }
    
    public void RestModStats(C_Model owner)
    {
        owner.AddDamage(-totalWeaponDamage);
        owner.AddAttackSpeed(-totalWeaponAttackSpeed);
        owner.AddCritRate(-totalCritRate);
        owner.AddCritDamage(-totalCritDamage);

        modDamage = 0;
        modAttackSpeed = 0f;
        modCritRate = 0f;
        modCritDamage = 0f;
    }

    public void Recalculate(C_Model owner)
    {
        totalWeaponDamage = baseWeaponDamage + modDamage;
        totalWeaponAttackSpeed = baseWeaponAttackSpeed + modAttackSpeed;
        totalCritRate = baseCritRate + modCritRate;
        totalCritDamage = baseCritDamage + modCritDamage;

        owner.AddDamage(totalWeaponDamage);
        owner.AddAttackSpeed(totalWeaponAttackSpeed);
        owner.AddCritRate(totalCritRate);
        owner.AddCritDamage(totalCritDamage);
    }

    public void AddSkill(int slot, SkillSO skillSO)
    {
        if (!weaponSkills.ContainsKey(slot))
        {
            weaponSkills.Add(slot, skillSO);
            skillSO.InitSkill(owner);
            Debug.Log($"스킬 {skillSO.skillName}이(가) 슬롯 {slot}에 추가되었습니다.");
        }
    }

    public void RemoveSkill(int slot)
    {
        if (weaponSkills.ContainsKey(slot))
        {
            weaponSkills.Remove(slot);
            Debug.Log($"슬롯 {slot}의 스킬이 제거되었습니다.");
        }
    }
}
