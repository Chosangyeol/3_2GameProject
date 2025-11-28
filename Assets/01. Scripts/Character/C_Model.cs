using Player.Item;
using Player.Weapon;
using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class C_Model : MonoBehaviour, IDamageable
    {
        public static C_Model Instance;

        [SerializeField]
        protected C_StatBaseSO statSO;
        protected C_StatBase statBase;

        [Header("Attack")]
        protected float _attackDelay;

        protected C_Inventory _inventory;
        protected C_WeaponSystem _weaponSystem;

        [Header("Å×½ºÆ®¿ë º¯¼ö")]
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

        private Animator _anim;
        public Animator Anim => _anim;

        public C_StatBase GetStat()
        {
            return statBase;
        }

        #region Unity Event
        protected virtual void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            cc = GetComponent<CharacterController>();
            if (statBase == null) statBase = new C_StatBase(statSO);
            _inventory = new C_Inventory(this);
            _weaponSystem = new C_WeaponSystem(this);
            _anim = GetComponentInChildren<Animator>();

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
        public void Damaged(int damage, GameObject attacker = null)
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
            Debug.Log("»ç¸Á");
        }

        public void Heal(int amount)
        {
            statBase.Heal(amount);
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

        #region Attack (ÄÞº¸)
        public void OpenCombo()
        {
            var ws = WeaponSystem;
            if (!ws.CurrentWeapon.attackBehavior.hasCombo) return;

            ws.attackOpen = true;
            ws.nextAttack = false;
        }

        public void CloseCombo()
        {
            var ws = WeaponSystem;

            if (!ws.CurrentWeapon.attackBehavior.hasCombo) return;

            ws.attackOpen = false;
        }

        public void EndAttack()
        {
            var ws = WeaponSystem;

            if (!ws.CurrentWeapon.attackBehavior.hasCombo)
            {
                canMove = false;
                return;
            }

            if (ws.nextAttack && ws.combo < 3)
            {
                ws.nextAttack = false;
                ws.attackOpen = false;

                ws.combo++;
                ws.isAttacking = true;

                ws.CurrentWeapon.attackBehavior.Execute(this, ws.CurrentWeapon);
            }
            else
            {
                // ÄÞº¸ Á¾·á
                ws.combo = 0;
                ws.nextAttack = false;
                ws.attackOpen = false;
                ws.isAttacking = false;
                canMove = true;
            }

        }


        #endregion
    }
}

