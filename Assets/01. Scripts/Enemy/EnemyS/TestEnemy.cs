using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : EnemyBase
{
    public override void Awake()
    {
        base.Awake();
        attackBehavior = new MeleeAttack();
    }
}
