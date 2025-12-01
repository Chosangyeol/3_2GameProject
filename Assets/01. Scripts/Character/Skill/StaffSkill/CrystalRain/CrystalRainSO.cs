using Player;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "SkillSO/CrystalRain")]
public class CrystalRainSO : SkillSO
{
    private CrystalRain crystalRain;
    public float skillDamage = 1.0f;

    public override void InitSkill(C_Model owner, float skillDamage = 1.0f)
    {
        crystalRain = new CrystalRain(owner, skillDamage);
    }

    public override void ActiveSkill()
    {
        Debug.Log("크리스탈 레인 스킬 사용");
        crystalRain.UseSkill(skillEffect);
    }
}
