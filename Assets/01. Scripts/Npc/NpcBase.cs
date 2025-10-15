using Player;
using System.Collections.Generic;
using UnityEngine;

public class NpcBase : InteractBase
{
    [Header("Npc Data")]
    public NpcSO npcData;

    protected Transform player;

    [Header("Npc 옵션 버튼")]
    public List<NpcButton> npcButtons = new List<NpcButton>();

    #region Override InteractBase
    public override void Interact(Transform interactor)
    {
        base.Interact(interactor);
        player = interactor;
        interactor.GetComponent<C_Model>().canAttack = false;
        UIManager.Instance.EnableUI(UIManager.Instance.npcUI);
        UIManager.Instance.EnableUI(UIManager.Instance.dialogUI);

        if (npcData != null)
            UIManager.Instance.dialogUI.GetComponent<DialogUI>().SetDialog(npcData);
        
        UIManager.Instance.dialogUI.GetComponent<DialogUI>().CreateButtons(npcButtons);
    }

    public override void OnUnfocus(Transform interactor)
    {
        base.OnUnfocus(interactor);
        interactor.GetComponent<C_Model>().canAttack = true;
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
