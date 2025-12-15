using NUnit.Framework;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Boss : BossBase
{
    [Header("패턴 1")]
    public Transform firePos;
    public PoolableMono pattern1Projectile;

    [Header("패턴 2")]
    public PoolableMono pattern2Warning;

    [Header("패턴 3")]
    public PoolableMono pattern3Projectile;
    public PoolableMono pattenr3Warning;

    [Header("특수 패턴")]
    public PoolableMono specialSafeZone;
    private PoolableMono safezone;
    private C_Model inSafe;
    public Transform mapCenter;

    protected override void Awake()
    {
        base.Awake();
        attackBehavior = new Stage1BossAttack(pattern1Projectile, firePos);
        mapCenter = GameObject.FindGameObjectWithTag("Boss1Center").transform;

        bossHpBar.maxValue = enemySO.maxHp;
        bossHpBar.value = enemySO.maxHp;
    }

    protected override void Update()
    {
        if (specialTrigger && !wasSpecial && !isAttack)
            StartCoroutine(Special());

        fsm.Tick();
    }

    

    public override void TakeDamage(int amount)
    {
        Stat.curHp -= amount;
        bossHpBar.value -= amount;

        damagedEffect.Play();

        if (Stat.curHp <= Stat.maxHp/2)
        {
            specialTrigger = true;
            Debug.Log("특수패턴 사용 가능");
        }

        if (Stat.curHp <= 0)
        {
            Die();
        }
    }

    IEnumerator Special()
    {
        wasSpecial = true;
        fsm.ChangeState(new State_BossSpecial(this, fsm, patternCount));

        yield return MoveToCenter(mapCenter.position);

        yield return new WaitForSeconds(2f);

        yield return JumpUp();

        CreateSafeZoen();

        yield return new WaitForSeconds(4f);

        yield return Down();

        agent.enabled = true;
        StartCoroutine(AttackDelay(2f));
        specialTrigger = false;
    }

    IEnumerator MoveToCenter(Vector3 center)
    {
        agent.enabled = false;

        Vector3 startPos = transform.position;

        if (Physics.Raycast(
        center + Vector3.up * 20f,
        Vector3.down,
        out RaycastHit hit,
        50f,
        LayerMask.GetMask("Ground")))
        {
            center = hit.point;
        }

        Vector3 midPos = (startPos + center) * 0.5f;
        midPos.y += 8f;
        float dur = 1.0f;
        float t = 0f;


        while (t < 1f)
        {
            t += Time.deltaTime / dur;
            transform.position = Bezier(startPos, midPos, center, t);

            // 회전 고정 (X,Z 기울기 방지)
            Vector3 euler = transform.eulerAngles;
            transform.rotation = Quaternion.Euler(0f, euler.y, 0f);

            yield return null;
        }

        transform.position = center;

        yield return new WaitForSeconds(0.2f);
    }
    IEnumerator JumpUp()
    {
        agent.enabled = false;

        Vector3 start = transform.position;
        Vector3 end = start + Vector3.up * 20f;

        float t = 0f;
        float duration = 0.5f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            transform.position = Vector3.Lerp(start, end, t);

            // 회전 고정 (중요)
            transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);

            yield return null;
        }
    }

    Vector3 safeZonePos;
    private void CreateSafeZoen()
    {
        safeZonePos = GetRandomGroundPoint(mapCenter.position, 20);

        PoolableMono safeZone = PoolManager.Instance.Pop(specialSafeZone.name);
        safezone = safeZone;
        safeZone.transform.position = safeZonePos;
    }

    IEnumerator Down()
    {
        agent.enabled = false;

        Vector3 start = transform.position;
        Vector3 end = start + Vector3.down * 20f;

        float t = 0f;
        float duration = 0.5f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            transform.position = Vector3.Lerp(start, end, t);

            // 회전 고정 (중요)
            transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);

            yield return null;
        }

        SpecialDamage(safeZonePos);
    }
    private void SpecialDamage(Vector3 safeCenter)
    {
        Collider[] player = Physics.OverlapSphere(mapCenter.position, 100f, LayerMask.GetMask("Player"));

        foreach (var col in player)
        {
            float dist = Vector3.Distance(col.transform.position, safeCenter);
            if (dist > 2.5f)
            {
                col.GetComponentInChildren<C_Model>().Damaged(100);
            }
        }

        PoolManager.Instance.Push(safezone);
    }

    Vector3 Bezier(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        return (1 - t) * (1 - t) * a
             + 2 * (1 - t) * t * b
             + t * t * c;
    }

    Vector3 GetRandomGroundPoint(Vector3 center, float radiu)
    {
        Vector3 rand = Random.insideUnitCircle * radiu;
        Vector3 pos = center + new Vector3(rand.x, 10f, rand.y);

        if (Physics.Raycast(pos, Vector3.down, out RaycastHit hit, 50f, LayerMask.GetMask("Ground")))
        {
            return hit.point;
        }
        return Vector3.zero;
    }

}
