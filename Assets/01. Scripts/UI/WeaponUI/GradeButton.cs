using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GradeButton : MonoBehaviour
{
    private C_Weapon weapon;
    public WeaponModSO currentMod;
    public TMP_Text modNameText;
    public int grade;
    public GameObject[] modButtons;

    public bool isOpen = false;

    private void OnEnable()
    {
        WeaponUI.Instance.OnSelectMod += CloseList;
        WeaponUI.Instance.OnTryModding += UpdateGradeButton;
        UpdateGradeButton();
    }

    private void OnDisable()
    {
        WeaponUI.Instance.OnSelectMod -= CloseList;
        WeaponUI.Instance.OnTryModding -= UpdateGradeButton;
        this.GetComponent<Button>().onClick.RemoveAllListeners();
    }

    public void UpdateGradeButton()
    {
        this.GetComponent<Button>().onClick.RemoveAllListeners();
        weapon = GameObject.FindGameObjectWithTag("Player").GetComponent<C_Model>().WeaponSystem.CurrentWeapon;
        if (!weapon.weaponModded.ContainsKey(grade))
        {
            modNameText.text = "∫Û ΩΩ∑Ì";
            this.GetComponent<Button>().onClick.AddListener(() =>
            {
                //Dotween Animation ¿€µø
                OpenList(weapon);
            });
        }
        else
        {
            currentMod = weapon.weaponModded[grade].weaponMod;
            modNameText.text = currentMod.modName;
            this.GetComponent<Button>().onClick.AddListener(() =>
            {
                WeaponUI.Instance.SelectMod(weapon.weaponModded[grade]);
            });
        }
    }

    public void OpenList(C_Weapon weapon)
    {
        if (isOpen)
        {
            for (int i = 0; i < modButtons.Length; i++)
            {
                modButtons[i].SetActive(false);
            }
            isOpen = false;
        }
        else
        {
            for (int i = 0; i < modButtons.Length; i++)
            {
                modButtons[i].SetActive(true);
                modButtons[i].GetComponent<ModButton>().InitModButton(weapon);
            }
            isOpen = true;
        }       
    }

    public void CloseList()
    {
        Debug.Log("Close List »£√‚");
        for (int i = 0; i < modButtons.Length; i++)
        {
            modButtons[i].SetActive(false);
        }
        isOpen = false;
    }
}
