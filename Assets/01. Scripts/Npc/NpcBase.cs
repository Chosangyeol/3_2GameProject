using System.Collections.Generic;
using UnityEngine;

public class NpcBase : InteractBase
{
    [Header("Npc Data")]
    public NpcSO npcData;

    [Header("Npc 옵션 버튼")]
    public List<NpcButton> npcButtons = new List<NpcButton>();

    #region Override InteractBase
    public override void Interact(Transform interactor)
    {
        base.Interact(interactor);
        UIManager.Instance.EnableUI(UIManager.Instance.npcUI);
        UIManager.Instance.EnableUI(UIManager.Instance.dialogUI);

        if (npcData != null)
            UIManager.Instance.dialogUI.GetComponent<DialogUI>().SetDialog(npcData);
        
        UIManager.Instance.dialogUI.GetComponent<DialogUI>().CreateButtons(npcButtons);
    }

    public override void OnUnfocus(Transform interactor)
    {
        base.OnUnfocus(interactor);
        UIManager.Instance.DisableUI(UIManager.Instance.npcUI);
        UIManager.Instance.DisableUI(UIManager.Instance.dialogUI);
    }

    protected override void Reset()
    {
        base.Reset();
        interactName = "F - Interact";
        priority = 0;
    }
    #endregion
}
