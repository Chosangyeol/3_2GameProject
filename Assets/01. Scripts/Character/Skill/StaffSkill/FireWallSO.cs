using Player;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "SkillSO/FireWallSO")]
public class FireWallSO : SkillSO
{
    private FireWall fireWall;
    public float skillDamage = 1.0f;

    public override void InitSkill(C_Model owner, float skillDamage = 1.0f)
    {
        fireWall = new FireWall();
    }

    public override void ActiveSkill()
    {
        Debug.Log("파이어월 스킬 사용");
    }
}
