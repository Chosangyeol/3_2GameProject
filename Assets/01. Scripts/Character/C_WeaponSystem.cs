using NUnit.Framework;
using Player;
using RPGCharacterAnims.Lookups;
using System.Collections;
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

        public int combo = 0;
        public bool isAttacking = false;
        public bool attackOpen = false;
        public bool nextAttack = false;

        public bool isSkill1Cool = false;
        public bool isSkill2Cool = false;

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
            if (weaponType == WeaponType.Staff)
            {
                currentWeapon = new C_Weapon(WeaponType.Staff);
                currentWeapon.RestModStats(_model);
                currentWeapon.Recalculate(_model);
            }
            else if (weaponType == WeaponType.Bow)
            {
                currentWeapon = new C_Weapon(WeaponType.Bow);
                currentWeapon.RestModStats(_model);
                currentWeapon.Recalculate(_model);
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

            currentWeapon.weaponModded.Add(modGrade, new WeaponModInstance { weaponMod = weaponMod });
            _model.GetStat().modingChance--;
            currentWeapon.weaponLevel++;

            
            currentWeapon.RestModStats(_model);
            foreach (var kv in currentWeapon.weaponModded)
            {
                var mod = kv.Value.weaponMod;
                mod.ActivateMod(currentWeapon, _model.GetStat());
            }
            currentWeapon.Recalculate(_model);
            Debug.Log("착용한 모드 : " + weaponMod.modName);
            return true;

        }

        public void ResetWeapon(C_Weapon weapon)
        {
            int returnChance = 0;
            returnChance = currentWeapon.weaponLevel - 1;
            _model.GetStat().modingChance += returnChance;
            currentWeapon.weaponLevel = 1;

            foreach (var kv in currentWeapon.weaponModded)
            {
                var mod = kv.Value.weaponMod;
                mod.DeactivateMod(currentWeapon, _model.GetStat());
            }

            currentWeapon.RestModStats(_model);
            currentWeapon.weaponModded.Clear();
            currentWeapon = null;
        }

        public void ResetWeapon()
        {
            ResetWeapon(currentWeapon);
        }

        public void ApplyMods(ref List<GameObject> projectiles, float speed)
        {
            var weapon = CurrentWeapon;
            if (weapon == null || weapon.weaponModded.Count == 0)
                return;

            foreach (var kv in weapon.weaponModded)
            {
                var mod = kv.Value.weaponMod;
                mod.OnFire(weapon, ref projectiles,speed);
            }
        }

        #endregion

        #region Skill

        public void UseSkill1()
        {
            if (CurrentWeapon == null) return;
            if (!currentWeapon.weaponSkills.ContainsKey(1))
                return;

            _model.StartCoroutine(Skill1Cooldown(5f));
            currentWeapon.weaponSkills[1].ActiveSkill();
        }

        public void UseSkill2()
        {
            if (CurrentWeapon == null) return;
            if (!currentWeapon.weaponSkills.ContainsKey(2))
                return;

            _model.StartCoroutine(Skill2Cooldown(5f));
            currentWeapon.weaponSkills[2].ActiveSkill();
        }

        IEnumerator Skill1Cooldown(float cool)
        {
            isSkill1Cool = true;
            Debug.Log("Skill 1 used, cooldown started.");
            yield return new WaitForSeconds(cool);
            Debug.Log("Skill 1 used, cooldown End.");
            isSkill1Cool = false;
        }

        IEnumerator Skill2Cooldown(float cool)
        {
            isSkill2Cool = true;
            Debug.Log("Skill 2 used, cooldown started.");
            yield return new WaitForSeconds(cool);
            Debug.Log("Skill 2 used, cooldown End.");
            isSkill2Cool = false;
        }

        #endregion
    }
}

