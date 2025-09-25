using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    public static UIManager Instance => instance;
    private static UIManager instance;

    [Header("Main UI List")]
    public GameObject gameUI;
    public GameObject playerUI;
    public GameObject npcUI;

    [Header("Sub UI-Player List")]
    public GameObject inventoryUI;

    [Header("Sub UI-Npc List")]
    public GameObject dialogUI;
    public GameObject weaponUI;
    public GameObject shopUI;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void EnableUI(GameObject ui)
    {
        if (ui.activeSelf) return;
        ui.SetActive(true);
    }

    public void DisableUI(GameObject ui)
    {
        if (!ui.activeSelf) return;
        ui.SetActive(false);
    }

    public void DisableUI_Npc(GameObject ui)
    {
        if (!ui.activeSelf) return;
        ui.SetActive(false);

        dialogUI.SetActive(false);
        weaponUI.SetActive(false);
        shopUI.SetActive(false);
    }

}
