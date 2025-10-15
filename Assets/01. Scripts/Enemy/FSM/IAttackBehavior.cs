using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackBehavior
{
    void ExecuteAttack(EnemyBase enemy);
}

public class MeleeAttack : IAttackBehavior
{
    public void ExecuteAttack(EnemyBase enemy)
    {
        //enemy.anim.SetTrigger("Attack");
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
    public void ExecuteAttack(EnemyBase enemy)
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