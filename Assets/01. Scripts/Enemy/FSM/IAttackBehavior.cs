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
        proj.GetComponent<EnemyProjectile>().damage = enemy.GetStat().damage;
        proj.transform.position = firePos.position;
        Vector3 targetPos = enemy.player.GetComponent<Collider>().bounds.center;
        proj.GetComponent<Rigidbody>().linearVelocity =
        (targetPos - firePos.position).normalized * proj.GetComponent<EnemyProjectile>().speed;
    }
}
