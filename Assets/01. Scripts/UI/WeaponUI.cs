using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using Player;
using TMPro;
using UnityEngine;

public class WeaponUI : MonoBehaviour
{
    private GameObject player;
    public GameObject weaponSelectUI;
    public GameObject moddingUI;

    public GameObject meleeModsPanel;
    public GameObject rangeModsPanel;

    public TMP_Text modName;
    public TMP_Text modDesc;


    public void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        UpdateWeaponState(player.transform);
    }

    public void OnDisable()
    {
        player = null;
        moddingUI.SetActive(false);
        weaponSelectUI.SetActive(false);
    }

    public void ExitModding()
    {
        this.gameObject.SetActive(false);
    }

    public void CreateRangeWeapon()
    {
        player.GetComponent<C_Model>().WeaponSystem.CreateWeapon(Enums.WeaponType.Range);
        weaponSelectUI.SetActive(false);
        UpdateWeaponState(player.transform);
    }

    public void CreateMeleeWeapon()
    {
        player.GetComponent<C_Model>().WeaponSystem.CreateWeapon(Enums.WeaponType.Melee);
        weaponSelectUI.SetActive(false);
        UpdateWeaponState(player.transform);
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

        if (player.GetComponent<C_Model>().WeaponSystem.CurrentWeapon.weaponType == Enums.WeaponType.Melee)
        {
            rangeModsPanel.SetActive(false);
             meleeModsPanel.SetActive(true);
        }
        else if (player.GetComponent<C_Model>().WeaponSystem.CurrentWeapon.weaponType == Enums.WeaponType.Range)
        {
            rangeModsPanel.SetActive(true);
            meleeModsPanel.SetActive(false);
        }
    }

    public void SelectMod(WeaponModSO modSO)
    {
        modName.text = modSO.modName;
        modDesc.text = modSO.modDesc;
    }

    #endregion
}
