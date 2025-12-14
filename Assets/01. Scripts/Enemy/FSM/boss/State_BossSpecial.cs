using UnityEngine;

public class State_BossSpecial : IState
{
    readonly EnemyBase enemy;
    readonly StateMachine fsm;
    readonly int patternCount;

    public State_BossSpecial(EnemyBase enemy, StateMachine fsm, int patternCount)
    {
        this.enemy = enemy;
        this.fsm = fsm;
        this.patternCount = patternCount;
    }

    public void OnEnter()
    {
        
    }

    public void Tick()
    {

    }

    public void FixedTick()
    {

    }

    public void OnExit()
    {

    }
}
