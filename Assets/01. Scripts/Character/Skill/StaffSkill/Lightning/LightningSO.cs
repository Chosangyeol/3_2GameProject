using Player;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "SkillSO/Lightning")]
public class LightningSO : SkillSO
{
    private Lightning lightning;
    public float skillDamage = 1.0f;

    public override void InitSkill(C_Model owner, float skillDamage = 1.0f)
    {
        lightning = new Lightning(owner, skillDamage);
    }

    public override void ActiveSkill()
    {
        Debug.Log("라이트닝 스킬 사용");
        lightning.UseSkill(skillEffect);
    }
}
