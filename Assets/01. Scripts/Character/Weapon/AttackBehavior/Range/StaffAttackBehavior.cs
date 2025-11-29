using NUnit.Framework;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffAttackBehavior : IPlayerAttackBe
{
    public bool hasCombo => false;

    public GameObject projectile;
    public float speed;
    public float maxChargeTime = 1f;
    private float maxSpeed = 1.5f;
    private bool isCharging;
    private float currentCharge;
    private Coroutine chargeRoutine;

    public StaffAttackBehavior(GameObject projectile)
    {
        this.projectile = projectile;
        this.speed = projectile.GetComponent<PlayerProjectile>().speed;
    }

    public void Execute(C_Model attacker, C_Weapon weapon)
    {
        
    }

    public void BeginCharge(C_Model attacker)
    {
        if (isCharging) return;
        isCharging = true;
        currentCharge = 0f;
        chargeRoutine = attacker.StartCoroutine(ChargeRoutine(attacker));
    }

    // 입력 시스템에서 마우스 떼었을 때
    public void ReleaseCharge(C_Model attacker)
    {
        if (!isCharging) return;
        isCharging = false;

        if (chargeRoutine != null)
            attacker.StopCoroutine(chargeRoutine);

        FireBall(attacker);
        attacker.StartAttackDelay();
    }

    // 충전 코루틴
    private IEnumerator ChargeRoutine(C_Model attacker)
    {
        attacker.canMove = false;
        while (isCharging)
        {
            currentCharge += Time.deltaTime;
            float ratio = Mathf.Clamp01(currentCharge / maxChargeTime);

            Vector3 dir = GetMouseDirection(attacker);
            attacker.transform.forward = dir;

            yield return null;

            currentCharge = Mathf.Min(currentCharge, maxChargeTime);
        }
    }

    private void FireBall(C_Model attacker)
    {
        float totalDamage = attacker.GetStat().damage;

        float ratio = Mathf.Clamp01(currentCharge / maxChargeTime);
        float finalSpeed = Mathf.Lerp(speed, speed*maxSpeed, ratio);
        float finalDamage = Mathf.Lerp(
            totalDamage,
            totalDamage * 1.7f,
            ratio);
        float ballSize = Mathf.Lerp(1f, 2f, ratio);

        Vector3 dir = GetMouseDirection(attacker);
        attacker.transform.forward = dir;

        PoolableMono proj = PoolManager.Instance.Pop(projectile.name);
        proj.transform.position = (attacker.transform.position + new Vector3(0,ballSize/2,0)) + dir * 1.2f;
        proj.transform.rotation = Quaternion.LookRotation(dir);
        proj.transform.localScale = Vector3.one * ballSize;

        Rigidbody rb = proj.GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // ProjectileDamage 컴포넌트가 있다면 데미지 전달
        if (proj.TryGetComponent(out PlayerProjectile pd))
            pd.damage = finalDamage;

        List<GameObject> projectiles = new List<GameObject> { proj.gameObject };
        attacker.WeaponSystem.ApplyMods(ref projectiles, finalSpeed);

        foreach (var p in projectiles)
        {
            if (p.TryGetComponent(out Rigidbody rb2))
                rb2.linearVelocity = p.transform.forward * finalSpeed;
        }

        attacker.canMove = true;
        currentCharge = 0f;
    }

    private Vector3 GetMouseDirection(C_Model model)
    {
        Camera cam = Camera.main;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        Plane groundPlane = new Plane(Vector3.up, Vector3.zero); // Y=0 평면 기준
        if (groundPlane.Raycast(ray, out float distance))
        {
            Vector3 point = ray.GetPoint(distance);
            Vector3 dir = (point - model.transform.position);
            dir.y = 0f;
            return dir.normalized;
        }

        return model.transform.forward;
    }
}
