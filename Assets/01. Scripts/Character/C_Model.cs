using Player.Item;
using Player.Weapon;
using System;
using System.Collections;
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
        protected C_Skill _skill;

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
        public C_Skill Skill { get => _skill; }

        public bool isAlive = true;
        public bool isMoveable { get; private set; }

        public event Action<C_StatBase> ActionCallbaskStatChanged;
        public event Action<C_StatBase> ActionCallbackItemChanged;

        private Animator _anim;
        public Animator Anim => _anim;

        public C_StatBase GetStat()
        {
            return statBase;
        }

        public void StartAttackDelay()
        {
            StartCoroutine(AttackDelay(statBase.attackSpeed));
        }

        IEnumerator AttackDelay(float delay)
        {
            canAttack = false;
            yield return new WaitForSeconds(delay);
            canAttack = true;
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
            _skill = new C_Skill(this);
            _anim = GetComponentInChildren<Animator>();

            canAttack = false;
        }

        protected virtual void Start()
        {

        }

        protected virtual void Update()
        {
            
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
            Debug.Log("사망");
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
            ActionCallbackItemChanged?.Invoke(statBase);
        }

        public void RemoveItem(AItem item)
        {
            if (_inventory.RemoveItem(item))
            {
                ActionCallbackItemChanged?.Invoke(statBase);
            }
            return;
        }

        public void AddGold(int amount)
        {
            statBase.money += amount;
            ActionCallbaskStatChanged?.Invoke(statBase);
        }

        public void UseGold(int amount)
        {
            statBase.money -= amount;
            ActionCallbaskStatChanged?.Invoke(statBase);
        }
        #endregion

        #region PlayerStat

        public void AddDamage(int amount)
        {
            statBase.AddDamage(amount);
            ActionCallbaskStatChanged?.Invoke(statBase);
        }

        public void AddMaxHp(int amount)
        {
            statBase.AddMaxHp(amount);
            ActionCallbaskStatChanged?.Invoke(statBase);
        }

        public void AddMoveSpeed(float amount)
        {
            statBase.AddMoveSpeed(amount);
            ActionCallbaskStatChanged?.Invoke(statBase);
        }

        public void AddAttackSpeed(float amount)
        {
            statBase.AddAttackSpeed(amount);
            ActionCallbaskStatChanged?.Invoke(statBase);
        }

        public void AddCritRate(float amount)
        {
            statBase.AddCritRate(amount);
            ActionCallbaskStatChanged?.Invoke(statBase);
        }

        public void AddCritDamage(float amount)
        {
            statBase.AddCritDamage(amount);
            ActionCallbaskStatChanged?.Invoke(statBase);
        }
        #endregion

        #region Skill

        public void UseSkill(int index)
        {
            if (index == 1)
            {
                if (Skill.isSkill1Cool) return;
                Skill.UseSkill1();
            }
            else if (index == 2)
            {
                if (Skill.isSkill2Cool) return;
                Skill.UseSkill2();
            }
        }

        #endregion
    }
}

