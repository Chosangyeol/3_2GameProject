using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class C_Movement : MonoBehaviour
{
    [Header("Movement Setting")]
    private C_StatBase stats;
    public float gravity = -20f;
    public Transform camTransform;

    private CharacterController cc;
    private Vector2 moveInput;
    private float verticalVel;

    [Header("Dash Setting")]
    public float dashSpeed = 14f;
    public float dashDuration = 0.25f;
    public float invulnDuration = 0.25f;
    public float cooldown = 1.0f;

    private bool canDash = true;
    private bool isDashing = false;

    private C_Health hp;

    [Header("테스트 용도")]
    public WeaponModSO testMod;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        hp = GetComponent<C_Health>();
        stats = gameObject.GetComponent<C_Health>().stats;
        

        if (camTransform == null && Camera.main != null)
            camTransform = Camera.main.transform;

        if (stats.weapon == null)
            stats.weapon = new C_Weapon(Enums.WeaponType.Range);
    }

    private void Update()
    {
        Vector3 camF = camTransform ? camTransform.forward : transform.forward;
        camF.y = 0f; camF.Normalize();
        Vector3 camR = camTransform ? camTransform.right : transform.right;
        camR.y = 0f; camR.Normalize();

        Vector3 wishDir = camF * moveInput.y + camR * moveInput.x;
        bool hasInput = wishDir.sqrMagnitude > 0.0001f;

        // 회전
        if (hasInput)
        {
            Quaternion targetRot = Quaternion.LookRotation(wishDir);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation, targetRot, stats.rotateSpeed * Time.deltaTime
            );
        }

        // 중력 & 이동
        verticalVel += gravity * Time.deltaTime;
        Vector3 velocity = wishDir.normalized * stats.moveSpeed + Vector3.up * verticalVel;

        CollisionFlags flags = cc.Move(velocity * Time.deltaTime);
        if ((flags & CollisionFlags.Below) != 0) verticalVel = -1f;

        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (!stats.weapon.TryModing(testMod, stats, out var reason))
            {
                Debug.Log("모드 장착 실패: " + reason);
            }
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            if (!stats.weapon.TryUpdradeMod(1, stats, out var reason))
            {
                Debug.Log("모드 업그레이드 실패: " + reason);
            }
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            stats.weapon.ResetModing(stats);
            Debug.Log("모드 초기화");
        }
    }

    public void Move(Vector2 move) => moveInput = move;

    public void TryDash()
    {
        if (canDash && !isDashing)
        {
            StartCoroutine(Dash());
        }
    }

    IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        hp.SetInvulnerable(true);

        float t = 0f;
        Vector3 forward = transform.forward;

        while (t < dashDuration)
        {
            cc.Move(forward * dashSpeed * Time.deltaTime);
            t += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(Mathf.Max(0f, invulnDuration - dashDuration));

        hp.SetInvulnerable(false);

        isDashing = false;

        yield return new WaitForSeconds(cooldown);
        canDash = true;
    }
}
