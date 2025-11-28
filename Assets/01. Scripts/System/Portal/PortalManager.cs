using UnityEngine;

public class PortalManager : MonoBehaviour
{
    private PortalManager instance;
    public PortalManager Instance => instance;

    [Header("ÇØ´ç ¸ÊÀÇ Æ÷Å»")]
    public Portal[] portalPos;

    private void Awake()
    {
        instance = this;

    }

    private void Start()
    {
        
    }

    public void OpenAllPortal()
    {
        for (int i = 0; i < portalPos.Length; i++)
        {
            portalPos[i].TogglePortal(true);
        }
    }
}
