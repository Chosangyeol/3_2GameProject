using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using NUnit.Framework;
using Player;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using Player.Weapon;
using UnityEngine.UI;
using System;

public class WeaponUI : MonoBehaviour
{
    public static WeaponUI Instance;

    private C_Model _model;
    private string reason;

    public GameObject weaponSelectUI;
    public GameObject moddingUI;

    public GameObject Grade1Button;
    public GameObject Grade2Button;
    public GameObject Grade3Button;
    public GameObject Grade4Button;

    public WeaponModSO selectMod;

    public event Action OnSelectMod;
    public event Action OnTryModding;

    public TMP_Text modName;
    public TMP_Text modDesc;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void OnEnable()
    {
        _model = GameObject.FindGameObjectWithTag("Player").GetComponent<C_Model>();
        UpdateWeaponState(_model.transform);
    }

    private void OnDisable()
    {
        _model = null;
        moddingUI.SetActive(false);
        weaponSelectUI.SetActive(false);
    }

    public void ExitModding()
    {
        this.gameObject.SetActive(false);
    }

    public void CreateBowWeapon()
    {
        _model.WeaponSystem.CreateWeapon(Enums.WeaponType.Bow);
        weaponSelectUI.SetActive(false);
        UpdateWeaponState(_model.transform);
    }

    public void CreateStaffWeapon()
    {
        _model.WeaponSystem.CreateWeapon(Enums.WeaponType.Staff);
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
        ShowCurrentWeaponMods();
    }

    public void ShowCurrentWeaponMods()
    {
        C_Weapon weapon = _model.WeaponSystem.CurrentWeapon;

        if (_model.WeaponSystem.CurrentWeapon.weaponLevel == 1)
        {
            Grade1Button.SetActive(true);
            Grade2Button.SetActive(false);
            Grade3Button.SetActive(false);
            Grade4Button.SetActive(false);
        }
        else if (_model.WeaponSystem.CurrentWeapon.weaponLevel == 2)
        {
            Grade1Button.SetActive(true);
            Grade2Button.SetActive(true);
            Grade3Button.SetActive(false);
            Grade4Button.SetActive(false);
        }
        else if (_model.WeaponSystem.CurrentWeapon.weaponLevel == 3)
        {
            Grade1Button.SetActive(true);
            Grade2Button.SetActive(true);
            Grade3Button.SetActive(true);
            Grade4Button.SetActive(false);
        }
        else if (_model.WeaponSystem.CurrentWeapon.weaponLevel == 4)
        {
            Grade1Button.SetActive(true);
            Grade2Button.SetActive(true);
            Grade3Button.SetActive(true);
            Grade4Button.SetActive(true);
        }


    }

    public void SelectMod(WeaponModInstance instance)
    {
        selectMod = instance.weaponMod;
        modName.text = selectMod.modName;
        modDesc.text = selectMod.modDesc;
    }

    public void SelectMod(WeaponModSO mod)
    {
        OnSelectMod?.Invoke();
        selectMod = mod;
        modName.text = selectMod.modName;
        modDesc.text = selectMod.modDesc;
    }

    public void TryModding()
    {
        _model.WeaponSystem.TryModing(selectMod,_model.WeaponSystem.CurrentWeapon,out reason);
        OnSelectMod?.Invoke();
        OnTryModding?.Invoke();
        ShowCurrentWeaponMods();
    }

    #endregion
}
