using UnityEngine;

namespace Player.Item
{
	public class DropItemModel : PoolableMono, IInteractable
    {
		[SerializeField]
		private AItemDataSO _itemDataSO;
        public AItem _item { get; private set; }
        public string InteractName { get; private set; }
        public int Priority { get; private set; }
        public Transform InteractTransform { get; private set; }

        public bool isInRange = false;
        public Transform player;

        public override void Reset()
        {
            base.Reset();
            InteractTransform = transform;
            InteractName = "F - " + _itemDataSO.itemName;
            Priority = 0;
        }

        private void OnEnable()
        {
            _item = _itemDataSO.CreateItem();
            Reset();
            Debug.Log(_item.itemData.itemName);
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isInRange = true;
                player = other.transform;
                OnFocus(player);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isInRange = false;
                player = null;
                OnUnfocus(player);
            }
        }

        #region Interact
        public void OnFocus(Transform interactor)
        {
            Debug.Log("아이템 줍기 가능");
        }

        public void OnUnfocus(Transform interactor)
        {
            Debug.Log("아이템 줍기 불가능");
        }

        public void Interact(Transform interactor)
        {
            Debug.Log("줍기");
            TryPickUp(interactor);
        }

        private void TryPickUp(Transform interactor)
        {
            C_Inventory inven = interactor.GetComponent<C_Model>().Inventory;

            inven.AddItem(_item);
            PoolManager.Instance.Push(this);
        }
        #endregion
    }
}
