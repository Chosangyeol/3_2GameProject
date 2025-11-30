using UnityEngine;

[CreateAssetMenu(fileName = "New WeaponMod", menuName = "WeaponMod/SkillMod")]
public class SkillModSO : WeaponModSO
{
    [Range(1,2)]
    public int skillSlot = 1;
    public SkillSO skillSO;

    public override void ActivateMod(C_Weapon weapon, C_StatBase owner)
    {
        base.ActivateMod(weapon, owner);
        weapon.AddSkill(skillSlot, skillSO);
    }

    public override void DeactivateMod(C_Weapon weapon, C_StatBase owner)
    {
        base.DeactivateMod(weapon, owner);
        weapon.RemoveSkill(skillSlot);
    }
}
