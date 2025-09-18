using UnityEngine;
using UnityEngine.InputSystem;

public class C_Movement : MonoBehaviour
{
    public C_StatBase stats;
    public float gravity = -20f;
    public Transform camTransform;
    private CharacterController cc;
    private Vector2 moveInput;
    private float verticalVel;

    public WeaponModSO testMod;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        if (stats == null) stats = new C_StatBase();
        stats.InitRuntime();

        if (camTransform == null && Camera.main != null)
            camTransform = Camera.main.transform;

        stats.weapon = new C_Weapon(Enums.WeaponType.Range);
    }

    void OnMove(InputValue v) => moveInput = v.Get<Vector2>();

    private void Update()
    {
        Vector3 camF = camTransform.forward; camF.y = 0f; camF.Normalize();
        Vector3 camR = camTransform.right; camR.y = 0f; camR.Normalize();

        Vector3 wishDir = camF * moveInput.y + camR * moveInput.x;
        bool hasInput = wishDir.sqrMagnitude > 0.0001f;

        if (hasInput)
        {
            Quaternion targetRot = Quaternion.LookRotation(wishDir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, stats.rotateSpeed * Time.deltaTime);
        }

        verticalVel += gravity * Time.deltaTime;
        Vector3 velocity = wishDir.normalized * stats.moveSpeed + Vector3.up * verticalVel;

        CollisionFlags flags = cc.Move(velocity * Time.deltaTime);
        if ((flags & CollisionFlags.Below) != 0) verticalVel = -1f;

        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (!stats.weapon.TryModing(1, testMod, stats, out var reason))
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
}
