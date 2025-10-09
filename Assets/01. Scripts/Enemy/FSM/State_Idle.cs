using UnityEngine;
using UnityEngine.AI;

public class State_Idle : IState
{
    readonly EnemyBase enemy;
    readonly StateMachine fsm;

    public State_Idle(EnemyBase enemy, StateMachine fsm)
    {
        this.enemy = enemy;
        this.fsm = fsm;
    }

    public void OnEnter()
    {
    }

    public void Tick()
    {
        float dist = Vector3.Distance(enemy.transform.position, enemy.player.position);
        if (dist <= enemy.enemySO.detectRange)
        {
            fsm.ChangeState(new State_Chase(enemy, fsm));
        }
    }

    public void FixedTick() { }

    public void OnExit() { }

}
