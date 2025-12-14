using NUnit.Framework;
using Player;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Stage1BossAttack : IAttackBehavior
{
    public PoolableMono pattern1Projectile;
    public Transform firePos;
    public Stage1BossAttack(PoolableMono pattern1Projectile, Transform firePos)
    {
        this.pattern1Projectile = pattern1Projectile;
        this.firePos = firePos;
    }

    public void ExecuteAttack(EnemyBase enemy, int patternIndex = 0)
    {
        if (enemy is BossBase boss)
        {
            Vector3 dir = (boss.player.transform.position - boss.transform.position);
            dir.y = 0;
            dir.Normalize();
            switch (patternIndex)
            {
                case 0:
                    Debug.Log(patternIndex + 1 + "번 패턴");
                    Pattern1(dir, boss);
                    enemy.StartCoroutine(boss.AttackDelay(boss.GetStat().attackSpeed));
                    break;
                case 1:
                    Debug.Log(patternIndex + 1 + "번 패턴");
                    enemy.StartCoroutine(Pattern2(dir, boss));
                    break;
                case 2:
                    Debug.Log(patternIndex + 1 + "번 패턴");
                    enemy.StartCoroutine(Pattern3(boss));
                    break;
            }
        }    
    }

    #region 패턴 1 - 8방향 가시
    private void Pattern1(Vector3 dir, BossBase enemy)
    {
        enemy.isAttack = true;
        for (int i = 0; i< 8; i++)
        {
            float angle = 45f * i;
            Quaternion rot = Quaternion.AngleAxis(angle, Vector3.up);
            Vector3 fireDir = rot * dir;

            PoolableMono proj = PoolManager.Instance.Pop(pattern1Projectile.gameObject.name);
            EnemyProjectile ep = proj.GetComponent<EnemyProjectile>();
            ep.owner = enemy;
            ep.damage = enemy.GetStat().damage;

            proj.transform.position = firePos.position;
            proj.transform.rotation = Quaternion.LookRotation(fireDir);

            Rigidbody rb = proj.GetComponent<Rigidbody>();
            rb.linearVelocity = fireDir * ep.speed;
        }

    }
    #endregion

    #region 패턴 2 - 점프 찍기
    IEnumerator Pattern2(Vector3 dir, BossBase enemy)
    {
        enemy.isAttack = true;
        Transform player = enemy.player;
        Vector3 startPos = enemy.transform.position;

        Vector3 playerAirPos = GetPlayerAbovePos(player);

        Vector3 mid = (startPos + playerAirPos) * 0.5f;
        mid.y += 6f;

        float dur = 0.8f;
        float t = 0f;

        // 애니메이션


        enemy.agent.updateRotation = false;
        enemy.agent.enabled = false;
        while (t < 1f)
        {
            t += Time.deltaTime / dur;
            enemy.transform.position = Bezier(startPos, mid, playerAirPos, t);
            LockRotationXZ(enemy.transform);
            yield return null;
        }

        enemy.transform.position = playerAirPos;

        if (enemy is Stage1Boss boss)
        {
            PoolableMono warning = PoolManager.Instance.Pop(boss.pattern2Warning.name);
            if (Physics.Raycast(
                enemy.transform.position,
                Vector3.down,
                out RaycastHit hit,
                50f,
                LayerMask.GetMask("Ground")))
            {
                warning.transform.position = hit.point;
            }
            else
                warning.transform.position = Vector3.zero;

            yield return new WaitForSeconds(0.5f);

            Collider[] col = Physics.OverlapCapsule(warning.transform.position, warning.transform.position + Vector3.up * 5f, 2.5f, LayerMask.GetMask("Player"));

            foreach (Collider coll in col)
            {
                if (coll.CompareTag("Player"))
                {
                    coll.GetComponentInChildren<C_Model>().Damaged(enemy.GetStat().damage);
                }
            }

            PoolManager.Instance.Push(warning);

        }


        float fallSpeed = 35f;

        while (true)
        {
            enemy.transform.position += Vector3.down * fallSpeed * Time.deltaTime;

            if (Physics.Raycast(
                enemy.transform.position,
                Vector3.down,
                out RaycastHit hit,
                1.5f,
                LayerMask.GetMask("Ground")))
            {
                enemy.transform.position = hit.point;
                break;
            }


            yield return null;
        }

        enemy.agent.enabled = true;
        enemy.agent.updateRotation = true;

        enemy.StartCoroutine(enemy.AttackDelay(enemy.GetStat().attackSpeed));
    }
    #endregion

    #region 패턴 3 - 점액 뿌리기
    IEnumerator Pattern3(BossBase enemy)
    {
        enemy.isAttack = true;

        List<Vector3> targets = GetGroundPoints(enemy.transform.position, 20f, 30);
        Vector3 startPos = enemy.transform.position + Vector3.up * 2;

        yield return new WaitForSeconds(1f);

        foreach (Vector3 target in targets)
        {
            if (enemy is Stage1Boss boss)
            {
                PoolableMono proj = PoolManager.Instance.Pop(boss.pattern3Projectile.name);
                proj.transform.position = startPos;

                PoolableMono warning = PoolManager.Instance.Pop(boss.pattenr3Warning.name);
                warning.transform.position = target;

                enemy.StartCoroutine(ArcProjectile(proj, startPos, target, 15f, 1.5f, warning,enemy));
            }
            yield return new WaitForSeconds(Random.Range(0f, 0.2f));
        }      
        yield return null;

        enemy.StartCoroutine(enemy.AttackDelay(enemy.GetStat().attackSpeed));
    }

    Vector3 GetRandomGroundPoint(Vector3 center, float radiu)
    {
        Vector3 rand = Random.insideUnitCircle * radiu;
        Vector3 pos = center + new Vector3(rand.x, 10f, rand.y);

        if (Physics.Raycast(pos,Vector3.down, out RaycastHit hit, 50f, LayerMask.GetMask("Ground")))
        {
            return hit.point;
        }
        return Vector3.zero;
    }

    List<Vector3> GetGroundPoints(Vector3 center, float radius, int count)
    {
        List<Vector3> points = new();

        for (int i = 0; i < count; i++)
        {
            Vector3 p = GetRandomGroundPoint(center, radius);
            if (p != Vector3.zero)
                points.Add(p);
        }
        return points;
    }

    IEnumerator ArcProjectile(
    PoolableMono projectile,
    Vector3 start,
    Vector3 target,
    float height,
    float duration, PoolableMono warning, EnemyBase enemy)
    {
        Vector3 mid = (start + target) * 0.5f;
        mid.y += height;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            projectile.transform.position = Bezier(start, mid, target, t);
            yield return null;
        }
        projectile.transform.position = target;

        Collider[] col = Physics.OverlapCapsule(warning.transform.position, warning.transform.position + Vector3.up * 5f, 1f, LayerMask.GetMask("Player"));

        foreach (Collider coll in col)
        {
            if (coll.CompareTag("Player"))
            {
                coll.GetComponent<C_Model>().Damaged(enemy.GetStat().damage);
            }
        }

        PoolManager.Instance.Push(warning);
        PoolManager.Instance.Push(projectile);
    }
    #endregion

    Vector3 GetPlayerAbovePos(Transform player)
    {
        Vector3 pos = player.position;

        // 플레이어 머리 위 (Collider 기준)
        Collider col = player.GetComponent<Collider>();
        if (col != null)
            pos.y = col.bounds.max.y + 3.0f; // 머리 위 2m
        else
            pos.y += 3f;

        return pos;
    }

    void LockRotationXZ(Transform t)
    {
        Vector3 euler = t.eulerAngles;
        t.rotation = Quaternion.Euler(0f, euler.y, 0f);
    }

    Vector3 Bezier(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        return (1 - t) * (1 - t) * a
             + 2 * (1 - t) * t * b
             + t * t * c;
    }
}
