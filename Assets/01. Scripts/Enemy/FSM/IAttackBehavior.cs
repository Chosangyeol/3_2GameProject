using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public interface IAttackBehavior
{
    void ExecuteAttack(EnemyBase enemy, int patternIndex = 0);
}

public class MeleeAttack : IAttackBehavior
{
    public void ExecuteAttack(EnemyBase enemy, int patternIndex = 0)
    {
        enemy.anim.SetTrigger("Attack");
    }
}

public class RangedAttack : IAttackBehavior
{
    public GameObject projectile;
    public Transform firePos;

    public RangedAttack(GameObject projectilePrefab, Transform firePos)
    {
        this.projectile = projectilePrefab;
        this.firePos = firePos;
    }
    public void ExecuteAttack(EnemyBase enemy, int patternIndex = 0)
    {
        //enemy.anim.SetTrigger("RangeAttack");
        PoolableMono proj = PoolManager.Instance.Pop(projectile.gameObject.name);
        proj.GetComponent<EnemyProjectile>().owner = enemy;
        proj.GetComponent<EnemyProjectile>().damage = enemy.GetStat().totalDamage;
        proj.transform.position = firePos.position;
        Vector3 targetPos = enemy.player.GetComponent<Collider>().bounds.center;
        proj.GetComponent<Rigidbody>().linearVelocity =
        (targetPos - firePos.position).normalized * proj.GetComponent<EnemyProjectile>().speed;
    }
}

public class Stage1BossAttack : IAttackBehavior
{
    public GameObject pattern1Projectile;
    public Transform firePos;
    public Stage1BossAttack(GameObject pattern1Projectile, Transform firePos)
    {
        this.pattern1Projectile = pattern1Projectile;
        this.firePos = firePos;
    }

    public void ExecuteAttack(EnemyBase enemy, int patternIndex = 0)
    {
        Vector3 dir = (enemy.player.transform.position - firePos.position).normalized;
        enemy.transform.forward = dir;
        switch (patternIndex)
        {
            case 0:
                Debug.Log(patternIndex + 1 + "번 패턴");
                Pattern1(dir, enemy);
                break;
            case 1:
                Debug.Log(patternIndex + 1 + "번 패턴");
                Pattenr2(dir, enemy);
                break;
            case 2:
                Debug.Log(patternIndex + 1 + "번 패턴");
                Pattern3(dir, enemy);
                break;
            case 3:
                Debug.Log(patternIndex + 1 + "번 패턴");
                Pattern4(dir, enemy);
                break;
        }
    }
    
    private void Pattern1(Vector3 dir, EnemyBase enemy)
    {
        PoolableMono proj = PoolManager.Instance.Pop(pattern1Projectile.gameObject.name);
        proj.GetComponent<EnemyProjectile>().owner = enemy;
        proj.GetComponent<EnemyProjectile>().damage = enemy.GetStat().totalDamage;
        proj.transform.position = firePos.position;
        proj.transform.rotation = Quaternion.LookRotation(dir);
        proj.GetComponent<Rigidbody>().linearVelocity =
        dir * proj.GetComponent<EnemyProjectile>().speed;
    }

    private void Pattenr2(Vector3 dir, EnemyBase enemy)
    {

    }

    private void Pattern3(Vector3 dir, EnemyBase enemy)
    {

    }
    private void Pattern4(Vector3 dir, EnemyBase enemy)
    {

    }
}