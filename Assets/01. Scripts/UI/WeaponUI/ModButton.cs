using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ModButton : MonoBehaviour
{
    public WeaponModSO currentMod;
    public TMP_Text modNameText;
    public WeaponModSO[] mods;

    private void OnEnable()
    {
        
    }

    public void InitModButton(C_Weapon weapon)
    {
        Enums.WeaponType type = weapon.weaponType;
        if (type == Enums.WeaponType.Melee)
        {
            currentMod = mods[0];
        }
        else if (type == Enums.WeaponType.Range)
        {
            currentMod = mods[1];
        }
        modNameText.text = currentMod.modName;
        this.GetComponent<Button>().onClick.RemoveAllListeners();
        this.GetComponent<Button>().onClick.AddListener(() =>
        {
            WeaponUI.Instance.SelectMod(currentMod);
        });
        Debug.Log("버튼 생성");
    }
}
