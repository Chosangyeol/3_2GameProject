using Player;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss2Attack : IAttackBehavior
{
    public Boss2Attack()
    {

    }

    public void ExecuteAttack(EnemyBase enemy, int patternIndex = 0)
    {
        if (enemy is not Boss2 boss) return;

        Debug.Log(patternIndex + 1 + "번 패턴");

        if (boss.canRush)
        {
            enemy.StartCoroutine(Pattern1(boss));
            return;
        }

        switch (patternIndex)
        {
            case 0:
                enemy.StartCoroutine(Pattern2(boss));
                break;

            case 1:
                enemy.StartCoroutine(Pattern3(boss));
                break;

            case 2:
                enemy.StartCoroutine(Pattern4(boss));
                break;
        }
    }

    #region 패턴 1 - 돌진 ( 특수 패턴에서 벽 부수는데 활용 )
    IEnumerator Pattern1(Boss2 enemy)
    {
        enemy.isAttack = true;

        bool hasHit = false;

        float t = 0;
        enemy.Lr.enabled = true;

        while (t < 1f && !enemy.IsDie)
        {
            t += Time.deltaTime;
            FacePlayer(enemy, enemy.player.position);
            yield return null;
        }
        enemy.Lr.enabled = false;


        Vector3 dir = enemy.player.position - enemy.transform.position;
        dir.y = 0;
        dir.Normalize();

        yield return new WaitForSeconds(1f);


        t = 0;
        while (t < 1 && !enemy.IsDie)
        {
            t += Time.deltaTime;

            if (Physics.CapsuleCast(
                enemy.transform.position + Vector3.up * 0.5f,
                enemy.transform.position + Vector3.up * 2.0f,
                1.5f,
                dir,
                out RaycastHit hit,
                1.0f,
                LayerMask.GetMask("Player", "BossObject")))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    hit.collider.GetComponentInChildren<C_Model>().Damaged(enemy.GetStat().damage);
                }

                if (hit.collider.CompareTag("Boss2Wall") && !hasHit)
                {
                    hit.collider.GetComponentInChildren<Boss2Wall>().WallCountDown();
                    hasHit = true;
                }

                break;
            }

            enemy.transform.position += dir * 15f * Time.deltaTime;
            yield return null;
        }

        enemy.canRush = false;

        enemy.StartCoroutine(enemy.AttackDelay(enemy.GetStat().attackSpeed));
    }

    private void FacePlayer(Boss2 enemy, Vector3 target)
    {
        Vector3 dir = (target - enemy.transform.position).normalized;
        dir.y = 0;
        enemy.transform.forward = dir;

        enemy.Lr.SetPosition(0, enemy.GetComponent<Collider>().bounds.center);
        enemy.Lr.SetPosition(1, enemy.player.GetComponent<Collider>().bounds.center);
    }

    #endregion

    #region 패턴 2 - 점프 -> 낙석
    IEnumerator Pattern2(Boss2 enemy)
    {
        enemy.isAttack = true;

        List<Vector3> targets = GetGroundPoints(enemy.boss2Center.position, 10f, 3);

        enemy.agent.updateRotation = false;
        enemy.agent.enabled = false;

        for (int i = 0; i < targets.Count; i++)
        {
            Vector3 startPos = enemy.transform.position;
            Vector3 endPos = targets[i];

            Vector3 mid = (startPos + endPos) * 0.5f;
            mid.y += 15f;

            float dur = 1.2f;
            float t = 0;

            FacePlayer(enemy, endPos);

            PoolableMono warning = PoolManager.Instance.Pop(enemy.boss2JumpWarning.name);
            warning.transform.position = endPos;

            while (t<1f)
            {
                t += Time.deltaTime / dur;
                enemy.transform.position = Bezier(startPos, mid, endPos, t);
                yield return null;
            }

            Collider[] col = Physics.OverlapSphere(warning.transform.position, 2.5f);

            foreach (Collider coll in col)
            {
                if (coll.CompareTag("Player"))
                {
                    coll.GetComponentInChildren<C_Model>().Damaged(enemy.GetStat().damage);
                }
            }

            PoolManager.Instance.Push(warning);

            // 낙석
            Stone(enemy);

            yield return new WaitForSeconds(1f);
        }

        enemy.agent.enabled = true;
        enemy.agent.updateRotation = true;

        enemy.StartCoroutine(enemy.AttackDelay(enemy.GetStat().attackSpeed));

    }

    public void Stone(Boss2 enemy)
    {
        List<Vector3> targets = GetGroundPoints(enemy.boss2Center.position, 15f, 8);

        for (int i = 0; i < targets.Count; i++)
        {
            PoolableMono obj = PoolManager.Instance.Pop(enemy.boss2Stone.name);
            obj.transform.position = targets[i];
        }
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

    #endregion

    #region 패턴 3 - 전방 부채꼴 타격
    IEnumerator Pattern3(Boss2 enemy)
    {
        enemy.isAttack = true;

        PoolableMono warning = PoolManager.Instance.Pop(enemy.boss2Pattern3Warning.name);
        warning.transform.position = enemy.transform.position;
        warning.transform.rotation = Quaternion.Euler(0,enemy.transform.eulerAngles.y - 112.5f,0);

        yield return new WaitForSeconds(1.5f);

        PoolManager.Instance.Push(warning);

        SectorAttack(enemy, 15f);
        enemy.StartCoroutine(enemy.AttackDelay(enemy.GetStat().attackSpeed));

    }

    void SectorAttack(Boss2 boss, float radius)
    {
        Vector3 origin = boss.transform.position;
        Vector3 forward = boss.transform.forward;
        forward.y = 0;
        forward.Normalize();

        float halfAngle = 22.5f; // 45도의 절반

        Collider[] hits = Physics.OverlapSphere(
            origin,
            radius,
            LayerMask.GetMask("Player")
        );

        foreach (Collider hit in hits)
        {
            Vector3 dirToTarget =
                hit.transform.position - origin;
            dirToTarget.y = 0;

            if (dirToTarget.sqrMagnitude < 0.01f)
                continue;

            dirToTarget.Normalize();

            float angle =
                Vector3.Angle(forward, dirToTarget);

            if (angle <= halfAngle)
            {
                C_Model player =
                    hit.GetComponentInChildren<C_Model>();

                if (player != null)
                {
                    player.Damaged(boss.GetStat().damage);
                }
            }
        }
    }
    #endregion

    #region 패턴 4 - 
    IEnumerator Pattern4(Boss2 enemy)
    {
        enemy.boss2Pattern4Warning.SetActive(true);
        float timer = 0f;

        float attackTimer = 0f;

        float rotateSpeed = 360f; // 초당 회전 각도
        float moveSpeed = 5f;

        enemy.agent.enabled = false;
        enemy.agent.updateRotation = false;

        while (timer < 4f && !enemy.IsDie)
        {
            timer += Time.deltaTime;

            Vector3 dir =
                enemy.player.position - enemy.transform.position;
            dir.y = 0f;

            if (dir.sqrMagnitude > 0.01f)
            {
                Quaternion targetRot =
                    Quaternion.LookRotation(dir.normalized);

                enemy.transform.rotation =
                    Quaternion.Euler(0, enemy.transform.eulerAngles.y+ rotateSpeed * Time.deltaTime, 0);
            }

            dir.Normalize();

            enemy.transform.position +=
                dir * moveSpeed * Time.deltaTime;

            attackTimer += Time.deltaTime;
            if (attackTimer >= 0.5f)
            {
                attackTimer = 0f;
                Pattern3Damage(enemy);
            }

            yield return null;
        }
        enemy.agent.enabled = true;
        enemy.agent.updateRotation = true;
        enemy.boss2Pattern4Warning.SetActive(false);


        enemy.StartCoroutine(enemy.AttackDelay(enemy.GetStat().attackSpeed));

    }

    private void Pattern3Damage(Boss2 enemy)
    {
        Collider[] hits = Physics.OverlapSphere(enemy.transform.position, 5f, LayerMask.GetMask("Player"));

        foreach (Collider c in hits)
        {
            C_Model player = c.GetComponentInChildren<C_Model>();
            if (player != null)
            {
                player.Damaged(enemy.GetStat().damage);
            }
        }
    }
    #endregion
    Vector3 Bezier(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        return (1 - t) * (1 - t) * a
             + 2 * (1 - t) * t * b
             + t * t * c;
    }
}
