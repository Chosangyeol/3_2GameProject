using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class C_Weapon
{
    public Enums.WeaponType weaponType;
    public float weaponDamage;
    public int weaponLevel;
    public Dictionary<int, WeaponModInstance> weaponModded = new Dictionary<int, WeaponModInstance>();

    public const int MinGrade = 1;
    public const int MaxGrade = 4;
    public C_Weapon(Enums.WeaponType weaponType)
    {
        this.weaponType = weaponType;
        if (weaponType == Enums.WeaponType.Range)
        {
            Debug.Log("���Ÿ� ���� ����");
            weaponDamage = 5f;
            weaponLevel = 1;
            if (weaponModded.Count > 0)
                weaponModded.Clear();
        }
        else if (weaponType == Enums.WeaponType.Melee)
        {
            Debug.Log("�ٰŸ� ���� ����");
            weaponDamage = 7f;
            weaponLevel = 1;
            if (weaponModded.Count > 0)
                weaponModded.Clear();
        }
    }

    public bool TryModing(int modGrade, WeaponModSO weaponMod, C_StatBase owner, out string reason)
    {
        reason = null;
        if (weaponMod == null)
        {
            reason = "��尡 �������� ����";
            return false;
        }

        if (modGrade < MinGrade || modGrade > MaxGrade)
        {
            reason = "��� �ܰ谡 �ùٸ��� ����";
            return false;        
        }

        if (weaponModded.ContainsKey(modGrade))
        {
            reason = "�̹� �ش� �ܰ��� ��尡 �����Ǿ� ����";
            return false;
        }

        if (weaponMod.allowedWeaponType != weaponType)
        {
            reason = "���� Ÿ�԰� ��� Ÿ���� ���� ����";
            return false;
        }

        if (owner.modingChance <= 0)
        {
            reason = "��� ��ȸ�� ����";
            return false;
        }
        
        weaponModded.Add(modGrade,new WeaponModInstance { weaponMod = weaponMod, level = 1 });
        owner.modingChance--;
        weaponLevel++;
        Recalculate(owner);
        return true;

    }

    public bool TryUpdradeMod(int modGrade, C_StatBase owner, out string reason)
    {
        reason = null;
        if (!weaponModded.TryGetValue(modGrade,out var inst))
        {
            reason = "�ش� �ܰ��� ��尡 �����Ǿ� ���� ����";
            return false;
        }
        if (inst.level >= inst.weaponMod.maxLevel)
        {
            reason = "��尡 �̹� �ִ� ������";
            return false;
        }
        if (owner.modingChance <= 0)
        {
            reason = "��� ��ȸ�� ����";
            return false;
        }

        inst.level++;
        owner.modingChance--;
        weaponLevel++;
        Recalculate(owner);
        return true;
    }

    public void ResetModing(C_StatBase owner)
    {
        int returnChance = 0;
        returnChance = weaponLevel - 1;
        owner.modingChance += returnChance;
        weaponLevel = 1;
        weaponModded.Clear();
        Recalculate(owner);
    }

    public void Recalculate(C_StatBase owner)
    {
        float baseDamage = (weaponType == Enums.WeaponType.Range) ? 5f : 7f;
        float addDamege = 0f;
        float attackSpeedM = 1f;

        foreach (var kv in weaponModded)
        {
            var inst = kv.Value;
            var mod = inst.weaponMod;
            int lv = inst.level;

            addDamege += (lv == 2 ? mod.addDamageLv2 : mod.addDamageLv1);
            attackSpeedM *= (lv == 2 ? mod.addAttackSpeedLv2 : mod.addAttackSpeedLv1);

        }

        weaponDamage = baseDamage + addDamege;
        owner.damage = weaponDamage;
        owner.attackSpeed = 1f * attackSpeedM;
        Debug.Log(owner.damage);
        Debug.Log(owner.attackSpeed);
        Debug.Log(weaponLevel);
    }
}
