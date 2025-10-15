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

        [Header("동작 스크립트")]
        [SerializeField] private C_Movement pMove;
        [SerializeField] private C_Interactor pInteractor;

        private void OnEnable()
        {
            EnableAction(MoveAction);
            EnableAction(DashAction);
            EnableAction(AttackAction);
            EnableAction(InteractAction);

            if (DashAction && DashAction.action != null)
                DashAction.action.started += OnDashStart;

            if (AttackAction && AttackAction.action != null)
            { 
                AttackAction.action.started += OnAttackStart;
                AttackAction.action.canceled += OnAttackEnd;

            }

            if (InteractAction && InteractAction.action != null)
                InteractAction.action.started += OnInteract;
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

            if (weapon.isChargeWeapon && weapon.attackBehavior is BowAttackBehavior charge)
            {
                charge.BeginCharge(_model);
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
        }

        private void OnInteract(InputAction.CallbackContext c)
        {
            pInteractor?.TryInteract();
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