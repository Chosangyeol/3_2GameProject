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

    public void WeaponModing(int modGrade, WeaponModSO weaponMod)
    {
       if (weaponMod == null)
       {
           Debug.LogWarning("��� �����Ͱ� �������� �ʽ��ϴ�. weaponMod is null");
           return;
       }

       if (modGrade < 1 ||  modGrade > 4)
       {
           Debug.LogWarning("��� �ܰ谡 �ùٸ��� �ʽ��ϴ�. modGrade < 1 or modGrade > 4");
           return;
       }

        if (weaponModded.ContainsKey(modGrade))
        {
            Debug.LogWarning("�̹� �����ܰ��� ��尡 �����Ǿ� �ֽ��ϴ�. weaponMod already modded.");
            return;
        }

        weaponModded.Add(modGrade, weaponMod);

    }

    public void ResetModing()
    {
        weaponModded.Clear();
    }
}
