using UnityEngine;

public class NpcBase : InteractBase
{
    //[Header("Npc Data")]
    

    #region Override InteractBase
    public override void Interact(Transform interactor)
    {
        base.Interact(interactor);
        UIManager.Instance.EnableUI(UIManager.Instance.npcUI);
        StartTalk();
    }

    public override void OnUnfocus(Transform interactor)
    {
        base.OnUnfocus(interactor);
        UIManager.Instance.DisableUI_Npc(UIManager.Instance.npcUI);

    }

    protected override void Reset()
    {
        base.Reset();
        interactName = "F - Interact";
        priority = 0;
    }
    #endregion

    #region NPC Base
    public void StartTalk()
    {
        UIManager.Instance.EnableUI(UIManager.Instance.dialogUI);
    }
    #endregion
}
