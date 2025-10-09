using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using Player;
using UnityEngine;

public class WeaponUI : MonoBehaviour
{
    private GameObject player;

    public void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        UpdateWeaponState(player.transform);
    }
    #region Weapon Moding UI
    public void UpdateWeaponState(Transform interacter)
    {
        C_Weapon weapon = interacter.GetComponent<C_Model>().GetStat().weapon;
        Debug.Log("무기 타입 : " + weapon.weaponType.ToString());
        Debug.Log("무기 데미지 : " + weapon.weaponDamage);
        Debug.Log("무기 레벨 : " + weapon.weaponLevel);
        for (int i = 1; i < 5; i++)
        {
            if (weapon.weaponModded.ContainsKey(i))
            {
                Debug.Log(i + "번 모드 : " + weapon.weaponModded[1].weaponMod.modName);
            }
            else
            {
                Debug.Log(i + "번 모드 : 없음");
            }
        }
    }
    #endregion
}
