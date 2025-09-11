using System.Collections;
using UnityEngine;

public class C_Dash : MonoBehaviour
{
    [Header("Dash Setting")]
    public float dashSpeed = 14f;
    public float dashDuration = 0.25f;
    public float invulnDuration = 0.25f;
    public float cooldown = 1.0f;

    private bool canDash = true;
    private bool isDashing = false;

    private C_Movement mover;
    private CharacterController cc;
    private C_Health hp;

    private void Awake()
    {
        mover = GetComponent<C_Movement>();
        cc = GetComponent<CharacterController>();
        hp = GetComponent<C_Health>();
    }

    void OnDash()
    {
        if (canDash && !isDashing)
            StartCoroutine(Dash());
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

        // Äð´Ù¿î
        yield return new WaitForSeconds(cooldown);
        canDash = true;
    }
}
