using UnityEngine;

public class WeaponNpc : NpcBase
{
    private void Awake()
    {
        npcButtons.Clear();

        NpcButton upgradeBtn = new NpcButton
        {
            buttonText = "무기 개조"
        };
        upgradeBtn.onClickEvent = new UnityEngine.Events.UnityEvent();
        upgradeBtn.onClickEvent.AddListener(OpenWeaponModingUI);

        NpcButton closeBtn = new NpcButton
        {
            buttonText = "대화 종료"
        };
        closeBtn.onClickEvent = new UnityEngine.Events.UnityEvent();
        closeBtn.onClickEvent.AddListener(CloseDialog);

        npcButtons.Add(upgradeBtn);
        npcButtons.Add(closeBtn);
    }

    private void OpenWeaponModingUI()
    {
        Debug.Log("무기 개조 창 오픈");
        UIManager.Instance.EnableUI(UIManager.Instance.weaponUI);
    }

    private void CloseDialog()
    {
        Debug.Log("대화 종료");
        UIManager.Instance.EnableUI(UIManager.Instance.weaponUI);
        UIManager.Instance.DisableUI(UIManager.Instance.dialogUI);
        UIManager.Instance.DisableUI(UIManager.Instance.npcUI);
    }
}
