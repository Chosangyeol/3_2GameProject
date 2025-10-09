using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using Player;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    public static UIManager Instance => instance;
    private static UIManager instance;

    private C_Model _model;

    [Header("Main UI List")]
    public GameObject gameUI;
    public GameObject playerUI;
    public GameObject npcUI;

    [Header("Sub UI-Player List")]
    public TMP_Text hpText;
    public TMP_Text mpText;
    public TMP_Text moneyText;
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

        _model = GameObject.FindGameObjectWithTag("Player").GetComponent<C_Model>();

    }

    private void Start()
    {
        UpdateUI(_model.GetStat());
    }

    private void OnEnable()
    {
        _model.ActionCallbaskStatChanged += UpdateUI;
    }

    private void OnDisable()
    {
        _model.ActionCallbaskStatChanged -= UpdateUI;
    }

    private void UpdateUI(C_StatBase statBase)
    {
        hpText.text = statBase.curHp + " / " + statBase.maxHp;
        moneyText.text = statBase.money + "G";
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
}
