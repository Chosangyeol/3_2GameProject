using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : EnemyBase
{
    protected override void Awake()
    {
        base.Awake();
        attackBehavior = new MeleeAttack();
    }
}
