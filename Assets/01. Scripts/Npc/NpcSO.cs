using UnityEngine;

[CreateAssetMenu(fileName = "New Npc Data", menuName = "SO/Npc Data")]
public class NpcSO : ScriptableObject
{
    public string npcName;
    public Dialog[] dialog;

}

[SerializeField]
public class Dialog
{
    public string[] sentences;
}
