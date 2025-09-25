using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
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
        C_Weapon weapon = interacter.GetComponent<C_Health>().stats.weapon;
        Debug.Log("���� Ÿ�� : " + weapon.weaponType.ToString());
        Debug.Log("���� ������ : " + weapon.weaponDamage);
        Debug.Log("���� ���� : " + weapon.weaponLevel);
        for (int i = 1; i < 5; i++)
        {
            if (weapon.weaponModded.ContainsKey(i))
            {
                Debug.Log(i + "�� ��� : " + weapon.weaponModded[1].weaponMod.modName);
            }
            else
            {
                Debug.Log(i + "�� ��� : ����");
            }
        }
    }
    #endregion
}
