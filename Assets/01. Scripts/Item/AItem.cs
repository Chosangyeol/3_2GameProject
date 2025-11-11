namespace Player.Item
{
	public abstract class AItem
	{
		public AItemDataSO itemData;

		public AItem(AItemDataSO itemData)
		{
			this.itemData = itemData;
			return ;
		}

		public abstract void OnAddInventory(C_Model model);

		public abstract bool OnUpdateInventory(C_Model model, float delta);

		public abstract void OnRemoveInventory(C_Model model);
	}
}
