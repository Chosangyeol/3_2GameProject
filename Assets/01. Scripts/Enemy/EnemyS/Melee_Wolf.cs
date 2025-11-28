using UnityEngine;

public class Melee_Wolf : EnemyBase
{
    protected override void Awake()
    {
        base.Awake();
        attackBehavior = new MeleeAttack();
    }
}
