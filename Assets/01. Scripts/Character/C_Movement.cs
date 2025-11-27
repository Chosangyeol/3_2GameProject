using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class C_Movement : MonoBehaviour
    {
        private C_Model _model;
        [Header("Movement Setting")]
        public float gravity = -20f;
        public Transform camTransform;

        private Vector2 moveInput;
        private float verticalVel;

        [Header("Dash Setting")]
        public float dashSpeed = 14f;
        public float dashDuration = 0.25f;
        public float invulnDuration = 0.25f;
        public float cooldown = 1.0f;

        private bool canDash = true;
        private bool isDashing = false;

        private void Start()
        {
            _model = GetComponent<C_Model>();

            if (camTransform == null && Camera.main != null)
                camTransform = Camera.main.transform;
        }

        private void Update()
        {
            Vector3 camF = camTransform ? camTransform.forward : _model.transform.forward;
            camF.y = 0f; camF.Normalize();
            Vector3 camR = camTransform ? camTransform.right : _model.transform.right;
            camR.y = 0f; camR.Normalize();

            Vector3 wishDir = camF * moveInput.y + camR * moveInput.x;
            bool hasInput = wishDir.sqrMagnitude > 0.0001f;
            if (!isDashing)
                _model.Anim.SetBool("isMove", hasInput);

            // 회전
            if (hasInput)
            {
                Quaternion targetRot = Quaternion.LookRotation(wishDir);
                _model.transform.rotation = Quaternion.RotateTowards(
                    _model.transform.rotation, targetRot, _model.GetStat().rotateSpeed * Time.deltaTime
                );
            }

            // 중력 & 이동
            if (_model.canMove)
            {
                verticalVel += gravity * Time.deltaTime;
                Vector3 velocity = wishDir.normalized * _model.GetStat().moveSpeed + Vector3.up * verticalVel;

                CollisionFlags flags = _model.cc.Move(velocity * Time.deltaTime);
                if ((flags & CollisionFlags.Below) != 0) verticalVel = -1f;
            }         
        }

        public void Move(Vector2 move) => moveInput = move;

        public void TryDash()
        {
            if (canDash && !isDashing)
            {
                _model.StartCoroutine(Dash());
            }
        }

        IEnumerator Dash()
        {
            canDash = false;
            isDashing = true;
            _model.Anim.SetTrigger("isDash");

            float t = 0f;
            Vector3 forward = _model.transform.forward;

            while (t < dashDuration)
            {
                _model.cc.Move(forward * dashSpeed * Time.deltaTime);
                t += Time.deltaTime;
                yield return null;
            }
            _model.Anim.SetTrigger("isDashEnd");

            yield return new WaitForSeconds(Mathf.Max(0f, invulnDuration - dashDuration));
            isDashing = false;

            yield return new WaitForSeconds(cooldown);
            canDash = true;
        }
    }
}
