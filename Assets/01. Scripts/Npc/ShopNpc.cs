using UnityEngine;

public class ShopNpc : NpcBase
{
    private void Awake()
    {
        npcButtons.Clear();

        NpcButton upgradeBtn = new NpcButton
        {
            buttonText = "상점"
        };
        upgradeBtn.onClickEvent = new UnityEngine.Events.UnityEvent();
        upgradeBtn.onClickEvent.AddListener(OpenShopUI);

        NpcButton closeBtn = new NpcButton
        {
            buttonText = "대화 종료"
        };
        closeBtn.onClickEvent = new UnityEngine.Events.UnityEvent();
        closeBtn.onClickEvent.AddListener(CloseDialog);

        npcButtons.Add(upgradeBtn);
        npcButtons.Add(closeBtn);
    }

    private void OpenShopUI()
    {
        Debug.Log("상점 오픈");
        UIManager.Instance.EnableUI(UIManager.Instance.shopUI);
    }

    private void CloseDialog()
    {
        Debug.Log("대화 종료");
        UIManager.Instance.DisableUI(UIManager.Instance.shopUI);
        UIManager.Instance.DisableUI(UIManager.Instance.dialogUI);
        UIManager.Instance.DisableUI(UIManager.Instance.npcUI);
    }
}
