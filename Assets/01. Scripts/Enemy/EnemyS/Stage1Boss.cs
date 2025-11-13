using UnityEngine;

public class Stage1Boss : BossBase
{

    public GameObject pattern1Projectile;
    public Transform firePos;
    protected override void Awake()
    {
        base.Awake();
        attackBehavior = new Stage1BossAttack(pattern1Projectile,firePos);
    }
}
