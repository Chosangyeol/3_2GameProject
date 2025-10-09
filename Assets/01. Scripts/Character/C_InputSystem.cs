using UnityEngine;
using UnityEngine.InputSystem;

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
                AttackAction.action.started += OnAttack;

            if (InteractAction && InteractAction.action != null)
                InteractAction.action.started += OnInteract;
        }

        private void Update()
        {
            Vector2 mv = MoveAction && MoveAction.action != null ? MoveAction.action.ReadValue<Vector2>() : Vector2.zero;
            if (pMove) pMove.Move(mv);
        }

        private void OnDashStart(InputAction.CallbackContext c)
        {
            pMove?.TryDash();
        }

        private void OnAttack(InputAction.CallbackContext c)
        {
            _model.WeaponSystem.Attack();
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


