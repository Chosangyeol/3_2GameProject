using UnityEngine;

[CreateAssetMenu(fileName = "New WeaponMod", menuName = "WeaponMod/SkillMod")]
public class SkillModSO : WeaponModSO
{
    public SkillSO skillSO;

    public override void ActivateMod(C_Weapon weapon, C_StatBase owner)
    {
        
    }

    public override void DeactivateMod(C_Weapon weapon, C_StatBase owner)
    {
        
    }
}
