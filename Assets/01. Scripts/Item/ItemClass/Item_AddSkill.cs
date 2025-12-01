using Player;
using Player.Item;
using UnityEngine;

public class Item_AddSkill : AItem
{
    private Item_AddSkillSO data;

    public Item_AddSkill(Item_AddSkillSO itemData) : base(itemData)
    {
        data = itemData;
    }

    public override void OnAddInventory(C_Model model)
    {
        Debug.Log(data.itemName + " √ﬂ∞°µ ");
        AddSkill(model,data.skillSO);
    }

    public override bool OnUpdateInventory(C_Model model, float delta)
    {
        return true;
    }

    public override void OnRemoveInventory(C_Model model)
    {
        Debug.Log(data.itemName + " ªË¡¶µ ");
        RemoveSkill(model,data.skillSO);
    }

    public void AddSkill(C_Model model, SkillSO skillSO)
    {
        model.Inventory.itemSkillSO = skillSO;
        model.Inventory.hasSkillItem = true;
        skillSO.InitSkill(model);
    }

    public void RemoveSkill(C_Model model, SkillSO skillSO)
    {
        model.Inventory.itemSkillSO = null;
        model.Inventory.hasSkillItem = false;
    }
}
