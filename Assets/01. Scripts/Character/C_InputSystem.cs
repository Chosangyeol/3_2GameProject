using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace Player
{
    public class C_InputSystem : MonoBehaviour
    {
        [SerializeField]
        private C_Model _model;

        public InputActionReference MoveAction;
        public InputActionReference DashAction;
        public InputActionReference AttackAction;
        public InputActionReference InteractAction;
        public InputActionReference SkillAction1;
        public InputActionReference SkillAction2;

        private C_Movement pMove;
        private C_Interactor pInteractor;


        private void Awake()
        {
            pMove = GetComponent<C_Movement>();
            pInteractor = GetComponent<C_Interactor>();
        }
        private void OnEnable()
        {
            EnableAction(MoveAction);
            EnableAction(DashAction);
            EnableAction(AttackAction);
            EnableAction(InteractAction);
            EnableAction(SkillAction1);
            EnableAction(SkillAction2);

            if (DashAction && DashAction.action != null)
                DashAction.action.started += OnDashStart;

            if (AttackAction && AttackAction.action != null)
            { 
                AttackAction.action.started += OnAttackStart;
                AttackAction.action.canceled += OnAttackEnd;

            }

            if (InteractAction && InteractAction.action != null)
                InteractAction.action.started += OnInteract;

            if (SkillAction1 && SkillAction1.action != null)
                SkillAction1.action.started += OnSkill1;

            if (SkillAction2 && SkillAction2.action != null)
                SkillAction2.action.started += OnSkill2;
        }

        private void OnDisable()
        {
            DisableAction(MoveAction);
            DisableAction(DashAction);
            DisableAction(AttackAction);
            DisableAction(InteractAction);
            DisableAction(SkillAction1);
            DisableAction(SkillAction2);

            if (DashAction && DashAction.action != null)
                DashAction.action.started -= OnDashStart;
            if (AttackAction && AttackAction.action != null)
            {
                AttackAction.action.started -= OnAttackStart;
                AttackAction.action.canceled -= OnAttackEnd;
            }
            if (InteractAction && InteractAction.action != null)
                InteractAction.action.started -= OnInteract;

            if (SkillAction1 && SkillAction1.action != null)
                SkillAction1.action.started -= OnSkill1;

            if (SkillAction2 && SkillAction2.action != null)
                SkillAction2.action.started -= OnSkill2;
        }

        private void Update()
        {
            Vector2 mv = MoveAction && MoveAction.action != null ? MoveAction.action.ReadValue<Vector2>() : Vector2.zero;
            if (pMove && _model.canMove) pMove.Move(mv);
        }

        private void OnDashStart(InputAction.CallbackContext c)
        {
            pMove?.TryDash();
        }

        private void OnAttackStart(InputAction.CallbackContext c)
        {
            var weapon = _model.WeaponSystem.CurrentWeapon;

            if (weapon == null || !_model.canAttack) return;

            if (weapon.isChargeWeapon && weapon.attackBehavior is BowAttackBehavior bow)
            {
                Debug.Log("Bow Charge Start");
                bow.BeginCharge(_model);
            }
            else if (weapon.isChargeWeapon && weapon.attackBehavior is StaffAttackBehavior staff)
            {
                Debug.Log("Staff Charge Start");
                staff.BeginCharge(_model);
            }
            else
            {
                _model.WeaponSystem.Attack();
            }
        }

        private void OnAttackEnd(CallbackContext c)
        {
            var weapon = _model.WeaponSystem.CurrentWeapon;
            if (weapon == null || !_model.canAttack) return;

            if (weapon.isChargeWeapon && weapon.attackBehavior is BowAttackBehavior charge)
            {
                charge.ReleaseCharge(_model);
            }
            else if (weapon.isChargeWeapon && weapon.attackBehavior is StaffAttackBehavior staff)
            {
                staff.ReleaseCharge(_model);
            }
        }

        private void OnInteract(InputAction.CallbackContext c)
        {
            pInteractor?.TryInteract();
        }

        private void OnSkill1(InputAction.CallbackContext c)
        {
            _model.UseSkill(1);
        }

        private void OnSkill2(InputAction.CallbackContext c)
        {
            _model.UseSkill(2);
        }

        private static void EnableAction(InputActionReference r)
        {
            if (r != null && r.action != null && !r.action.enabled) r.action.Enable();
        }
        private static void DisableAction(InputActionReference r)
        {
            if (r != null && r.action != null && r.action.enabled) r.action.Disable();
        }
    }
}