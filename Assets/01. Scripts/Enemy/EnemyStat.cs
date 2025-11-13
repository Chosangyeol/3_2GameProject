using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat
{
    public EnemySO enemySO;
    public int maxHp;
    public int curHp;

    public int baseDamage;
    public int totalDamage;

    public float moveSpeed;
    public float attackSpeed;

    public EnemyStat(EnemySO enemySO)
    {
        EnemyReset(enemySO);
    }

    public void EnemyReset(EnemySO enemySO)
    {
        this.enemySO = enemySO;
        this.maxHp = enemySO.maxHp;
        this.curHp = maxHp;
        this.baseDamage = enemySO.damage;
        this.moveSpeed = enemySO.moveSpeed;
        this.attackSpeed = enemySO.attackSpeed;
    }
}
