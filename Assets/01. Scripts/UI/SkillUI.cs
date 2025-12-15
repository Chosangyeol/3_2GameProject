using Player;
using Player.Weapon;
using RPGCharacterAnims.Lookups;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    public Image skill1Image; 
    public Image skill2Image; 
    public Image skill3Image;

    public C_Model player;
    private C_Weapon weapon;

    private void OnEnable()
    {
        C_Model.OnPlayerReady += OnPlayerReady;
        
    }

    private void OnDisable()
    {
        C_Model.OnPlayerReady -= OnPlayerReady;
        player.WeaponSystem.OnWeaponReady -= OnWeaponReady;

        player.Inventory.OnSkillAdd -= AddInvenSkillToUI;
        player.Inventory.OnSkillRemove -= RemoveInvenSkillToUI;

        player.WeaponSystem.CurrentWeapon.OnSkillAdd -= AddSkillToUI;
        player.WeaponSystem.CurrentWeapon.OnSkillRemove -= RemoveSkillToUI;

    }

    private void OnPlayerReady(C_Model model)
    {
        player = model;

        player.Inventory.OnSkillAdd += AddInvenSkillToUI;
        player.Inventory.OnSkillRemove += RemoveInvenSkillToUI;

        player.WeaponSystem.OnWeaponReady += OnWeaponReady;
    }

    private void OnWeaponReady(C_Weapon weapon)
    {
        weapon.OnSkillAdd += AddSkillToUI;
        weapon.OnSkillRemove += RemoveSkillToUI;
    }

    public void AddSkillToUI(int slot, SkillSO skill)
    {
        if (slot == 1)
        {
            skill1Image.sprite = skill.skillImage;
            skill1Image.fillAmount = 1;  
            player.WeaponSystem.OnSkill1Cooldown += SkillCool;
        }
        else if (slot == 2)
        {
            skill2Image.sprite = skill.skillImage;
            skill2Image.fillAmount = 1;
            player.WeaponSystem.OnSkill2Cooldown += SkillCool;
        }
        else
        {
            Debug.Log("알 수 없는 Slot 번호 입니다.");
        }

    }

    public void RemoveSkillToUI(int slot)
    {
        if (slot == 1)
        {
            skill1Image.sprite = null;
            player.WeaponSystem.OnSkill1Cooldown -= SkillCool;
        }
        else if (slot == 2)
        {
            skill2Image.sprite = null;
            player.WeaponSystem.OnSkill1Cooldown -= SkillCool;
        }
        else
        {
            Debug.Log("알 수 없는 Slot 번호 입니다.");
        }
    }

    public void AddInvenSkillToUI(SkillSO skill)
    {
        skill3Image.sprite = skill.skillImage;
        skill3Image.fillAmount = 1;
        player.Inventory.OnSkill3Cool += SkillCool;
    }

    public void RemoveInvenSkillToUI()
    {
        skill3Image.sprite = null;
        player.Inventory.OnSkill3Cool -= SkillCool;
    }

    public void SkillCool(int slot, float cool)
    {
        StartCoroutine(SkillCoolDownUI(slot, cool)) ;
    }

    IEnumerator SkillCoolDownUI(int slot, float cool)
    {
        float t = 0f;

        Image targetImage = null;

        switch (slot)
        {
            case 1: targetImage = skill1Image; break;
            case 2: targetImage = skill2Image; break;
            case 3: targetImage = skill3Image; break;
        }

        if (targetImage == null)
            yield break;

        // 쿨타임 시작 → 비어있는 상태
        targetImage.fillAmount = 0f;

        while (t < cool)
        {
            t += Time.deltaTime;
            targetImage.fillAmount = t / cool;
            yield return null;
        }

        // 보정 (정확히 100%)
        targetImage.fillAmount = 1f;
    }
}