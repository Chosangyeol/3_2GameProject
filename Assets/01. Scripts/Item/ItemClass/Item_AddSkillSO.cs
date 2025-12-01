using Player.Item;
using UnityEngine;

[CreateAssetMenu(fileName = "AddStat Item", menuName = "SO/Data - Item/AddSkill")]
public class Item_AddSkillSO : AItemDataSO
{
    public SkillSO skillSO;

    public override AItem CreateItem()
    {
        return new Item_AddSkill(this);
    }
}
