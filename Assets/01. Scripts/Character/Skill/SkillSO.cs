using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "SkillSO")]
public class SkillSO : ScriptableObject
{
    [Header("Skill Info")]
    public string skillName;
    public string skillDesc;
    public float skillCooldown;

    public virtual void ActiveSkill() { }
}
