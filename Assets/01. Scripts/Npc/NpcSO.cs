using UnityEngine;

[CreateAssetMenu(fileName = "New Npc Data", menuName = "SO/Npc Data")]
public class NpcSO : ScriptableObject
{
    public string npcName;

    [SerializeField]
    public Dialog[] dialog;

}

[System.Serializable]
public class Dialog
{
    [TextArea(2, 5)]
    public string[] sentences;
}
