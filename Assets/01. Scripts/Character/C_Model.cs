using Player.Item;
using Player.Weapon;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class C_Model : MonoBehaviour, IDamageable
    {
        [SerializeField]
        protected C_StatBaseSO statSO;
        protected C_StatBase statBase;

        [Header("Attack")]
        protected float _attackDelay;

        protected C_Inventory _inventory;
        protected C_WeaponSystem _weaponSystem;

        [Header("테스트용 변수")]
        public List<WeaponModSO> modList1;
        public List<WeaponModSO> modList2;
        private int weaponindex1 = 0;
        private int weaponindex2 = 0;
        private string reason;

        [HideInInspector]
        public CharacterController cc;
        public bool canAttack = false;
        public bool canMove = true;

        public C_Inventory Inventory { get => _inventory; }
        public C_WeaponSystem WeaponSystem { get => _weaponSystem; }

        public bool isAlive = true;
        public bool isMoveable { get; private set; }

        public event Action<C_StatBase> ActionCallbaskStatChanged;
        public event Action ActionCallbackItemChanged;

        public C_StatBase GetStat()
        {
            return statBase;
        }

        #region Unity Event
        protected virtual void Awake()
        {
            cc = GetComponent<CharacterController>();
            if (statBase == null) statBase = new C_StatBase(statSO);
            _inventory = new C_Inventory(this);
            _weaponSystem = new C_WeaponSystem(this);

            canAttack = false;
        }

        protected virtual void Start()
        {

        }

        protected virtual void Update()
        {
            if (Input.GetKeyDown(KeyCode.O) && weaponindex1 < 4)
            {
                WeaponSystem.TryModing(modList1[weaponindex1], _weaponSystem.CurrentWeapon, out reason);
                weaponindex1++;
            }

            if (Input.GetKeyDown(KeyCode.P) && weaponindex2 < 4)
            {
                WeaponSystem.TryModing(modList2[weaponindex2], _weaponSystem.CurrentWeapon, out reason);
                weaponindex2++;
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                WeaponSystem.ResetModing(_weaponSystem.CurrentWeapon);
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                Damaged(10);
            }
        }
        #endregion

        #region Damaged
        public void Damaged(float damage, GameObject attacker = null)
        {
            statBase.curHp -= damage;
            Debug.Log(statBase.curHp);
            ActionCallbaskStatChanged?.Invoke(statBase);
            if (statBase.curHp <= 0 && isAlive == true)
                Die();
        }

        public void Die()
        {
            isAlive = false;
            Debug.Log("사망");
        }

        public void Heal(float amount)
        {
            statBase.curHp += amount;
            if (statBase.curHp > statBase.maxHp)
                statBase.curHp = statBase.maxHp;
        }
        #endregion

        #region C_Item / Inventory
        public void AddItem(AItem item)
        {
            _inventory.AddItem(item);
            ActionCallbackItemChanged?.Invoke();
        }

        public void RemoveItem(AItem item)
        {
            if (_inventory.RemoveItem(item))
            {
                ActionCallbackItemChanged?.Invoke();
            }
            return;
        }
        #endregion
    }
}

