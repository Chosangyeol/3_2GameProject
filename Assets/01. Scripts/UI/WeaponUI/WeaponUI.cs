using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using NUnit.Framework;
using Player;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using Player.Weapon;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    private C_Model _model;
    private string reason;

    public GameObject weaponSelectUI;
    public GameObject moddingUI;

    public GameObject meleeParent;
    public GameObject rangeParent;
    public GameObject[] meleeButtons;
    public GameObject[] rangeButtons;

    public GameObject meleeModsPanel;
    public GameObject rangeModsPanel;

    public WeaponModSO selectMod;

    public List<WeaponModSO> bowMods;

    public TMP_Text modName;
    public TMP_Text modDesc;


    public void OnEnable()
    {
        _model = GameObject.FindGameObjectWithTag("Player").GetComponent<C_Model>();
        UpdateWeaponState(_model.transform);
    }

    public void OnDisable()
    {
        _model = null;
        moddingUI.SetActive(false);
        weaponSelectUI.SetActive(false);
    }

    public void ExitModding()
    {
        this.gameObject.SetActive(false);
    }

    public void CreateRangeWeapon()
    {
        _model.WeaponSystem.CreateWeapon(Enums.WeaponType.Range);
        weaponSelectUI.SetActive(false);
        UpdateWeaponState(_model.transform);
    }

    public void CreateMeleeWeapon()
    {
        _model.WeaponSystem.CreateWeapon(Enums.WeaponType.Melee);
        weaponSelectUI.SetActive(false);
        UpdateWeaponState(_model.transform);
    }

    #region Weapon Moding UI
    public void UpdateWeaponState(Transform interacter)
    {
        if (interacter.GetComponent<C_Model>().WeaponSystem.CurrentWeapon == null)
        {
            weaponSelectUI.SetActive(true);
        }
        else
        {
            OpenModdingUI(interacter);
        }  
    }

    public void OpenModdingUI(Transform interacter)
    {
        weaponSelectUI.SetActive(false);
        moddingUI.SetActive(true);

        if (_model.WeaponSystem.CurrentWeapon.weaponType == Enums.WeaponType.Melee)
        {
            rangeModsPanel.SetActive(false);
            meleeModsPanel.SetActive(true);
            ShowCurrentWeaponMods();
        }
        else if (_model.WeaponSystem.CurrentWeapon.weaponType == Enums.WeaponType.Range)
        {
            rangeModsPanel.SetActive(true);
            meleeModsPanel.SetActive(false);
            ShowCurrentWeaponMods();
        }
    }

    public void ShowCurrentWeaponMods()
    {
        C_Weapon weapon = _model.WeaponSystem.CurrentWeapon;

        if (weapon.weaponLevel >= 2)
        {
            if (weapon.weaponType == Enums.WeaponType.Melee)
            {
                ShowMeleeMods(weapon);
            }
            else if (weapon.weaponType == Enums.WeaponType.Range)
            {
                ShowRangeMods(weapon);
            }
        }
        else
        {
            meleeParent.SetActive(false);
            rangeParent.SetActive(false);
        }
    }

    public void ShowRangeMods(C_Weapon weapon)
    {
        rangeParent.SetActive(true);
        meleeParent.SetActive(false);
        if (weapon.attackBehavior is BowAttackBehavior bow)
        {
            for (int i = 0; i < 9; i++)
            {
                int index = i;

                rangeButtons[index].GetComponent<ModButton>().modNameText.text = bowMods[index].modName;
                Button btn = rangeButtons[index].GetComponent<Button>();
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() => SelectMod(bowMods[index]));
            }
        }
    }

    public void ShowMeleeMods(C_Weapon weapon)
    {
        rangeParent.SetActive(false);
        meleeParent.SetActive(true);
        Debug.Log("근접 무기 모드");
    }

    public void SelectMod(WeaponModSO modSO)
    {
        selectMod = modSO;
        modName.text = modSO.modName;
        modDesc.text = modSO.modDesc;
    }

    public void TryModding()
    {
        _model.WeaponSystem.TryModing(selectMod,_model.WeaponSystem.CurrentWeapon,out reason);
        ShowCurrentWeaponMods();
    }

    #endregion
}
