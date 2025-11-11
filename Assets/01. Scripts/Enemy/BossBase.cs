using UnityEngine;

public class BossBase : EnemyBase
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnEnable()
    {
        // fsm.ChangeState(new State_Idle(this, fsm));
    }
}
