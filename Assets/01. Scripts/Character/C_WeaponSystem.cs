using NUnit.Framework;
using Player;
using System.Collections.Generic;
using UnityEngine;
using static Enums;

namespace Player.Weapon
{
    public class C_WeaponSystem
    {
        private readonly C_Model _model;
        private C_Weapon currentWeapon;

        public C_Weapon CurrentWeapon { get => currentWeapon; }

        public const int MinGrade = 1;
        public const int MaxGrade = 4;

        public C_WeaponSystem(C_Model model)
        {
            this._model = model;
            return;
        }



        public void Attack()
        {
            if (!_model.canAttack)
                return;
            currentWeapon.attackBehavior.Execute(_model, currentWeapon);
        }

        public void CreateWeapon(WeaponType weaponType)
        {
            if (weaponType == WeaponType.Range)
            {
                currentWeapon = new C_Weapon(WeaponType.Range);
            }
            else if (weaponType == WeaponType.Melee)
            {
                currentWeapon = new C_Weapon(WeaponType.Melee);
            }
        }

        #region Weapon Moding
        public bool TryModing(WeaponModSO weaponMod, C_Weapon weapon, out string reason)
        {
            int modGrade = weaponMod.modGrade;
            reason = null;
            if (weaponMod == null)
            {
                reason = "모드가 존재하지 않음";
                return false;
            }

            if (modGrade < MinGrade || modGrade > MaxGrade)
            {
                reason = "모드 단계가 올바르지 않음";
                return false;
            }

            if (currentWeapon.weaponModded.ContainsKey(modGrade))
            {
                reason = "이미 해당 단계의 모드가 장착되어 있음";
                return false;
            }

            if (weaponMod.allowedWeaponType != currentWeapon.weaponType)
            {
                reason = "무기 타입과 모드 타입이 맞지 않음";
                return false;
            }

            if (_model.GetStat().modingChance <= 0)
            {
                reason = "모딩 기회가 없음";
                return false;
            }

            currentWeapon.weaponModded.Add(modGrade, new WeaponModInstance { weaponMod = weaponMod, level = 1 });
            _model.GetStat().modingChance--;
            currentWeapon.weaponLevel++;
            weaponMod.ApplyMod(currentWeapon, _model.GetStat(), currentWeapon.weaponModded[modGrade].level);
            Debug.Log("착용한 모드 : " + weaponMod.modName);
            Recalculate(weapon);
            return true;

        }

        public bool TryUpdradeMod(int modGrade, C_Weapon weapon, out string reason)
        {
            reason = null;
            if (!currentWeapon.weaponModded.TryGetValue(modGrade, out var inst))
            {
                reason = "해당 단계의 모드가 장착되어 있지 않음";
                return false;
            }
            if (inst.level >= inst.weaponMod.maxLevel)
            {
                reason = "모드가 이미 최대 레벨임";
                return false;
            }
            if (_model.GetStat().modingChance <= 0)
            {
                reason = "모딩 기회가 없음";
                return false;
            }

            inst.level++;
            _model.GetStat().modingChance--;
            currentWeapon.weaponLevel++;
            Recalculate(weapon);
            return true;
        }

        public void ResetModing(C_Weapon owner)
        {
            int returnChance = 0;
            returnChance = currentWeapon.weaponLevel - 1;
            _model.GetStat().modingChance += returnChance;
            currentWeapon.weaponLevel = 1;
            currentWeapon.weaponModded.Clear();
            Recalculate(owner);
        }

        public void ResetWeapon()
        {
            if (currentWeapon != null)
            {

                currentWeapon = null;
            }
            else return;         
        }

        public void ApplyMods(ref List<GameObject> projectiles, float speed)
        {
            var weapon = CurrentWeapon;
            if (weapon == null || weapon.weaponModded.Count == 0)
                return;

            foreach (var kv in weapon.weaponModded)
            {
                var mod = kv.Value.weaponMod;
                mod.OnFire(weapon, ref projectiles,speed,kv.Value.level);
            }
        }

        public void Recalculate(C_Weapon owner)
        {
            float baseDamage = (currentWeapon.weaponType == Enums.WeaponType.Range) ? 5f : 7f;
            float addDamege = 0f;
            float attackSpeedM = 1f;

            foreach (var kv in currentWeapon.weaponModded)
            {
                var inst = kv.Value;
                var mod = inst.weaponMod;
                int lv = inst.level;

                mod.ApplyMod(currentWeapon, _model.GetStat(), lv);

            }

            currentWeapon.weaponDamage = baseDamage + addDamege;
            owner.weaponDamage = currentWeapon.weaponDamage;
            owner.weaponAttackSpeed = 1f * attackSpeedM;
            Debug.Log(owner.weaponDamage);
            Debug.Log(owner.weaponAttackSpeed);
            Debug.Log(currentWeapon.weaponLevel);
        }
        #endregion
    }
}

