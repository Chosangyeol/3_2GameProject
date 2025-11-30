using Player;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "SkillSO")]
public class SkillSO : ScriptableObject
{
    [Header("Skill Info")]
    public string skillName;
    public string skillDesc;
    public float skillCooldown;
    public GameObject skillEffect;

    public virtual void InitSkill(C_Model owner, float skillDamage = 1.0f) { }
    public virtual void ActiveSkill() { }
}
