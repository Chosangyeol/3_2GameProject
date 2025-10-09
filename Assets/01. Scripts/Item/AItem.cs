namespace Player.Item
{
    public abstract class AItem
    {
        public ItemBaseSO itemSO;

        public AItem(ItemBaseSO itemSO)
        {
            this.itemSO = itemSO;
            return;
        }

        public abstract void OnAddInventory(C_Model model);
        public abstract bool OnUpdateInventory(C_Model model, float delta);
        public abstract void OnRemoveInventory(C_Model model);
    }
}



