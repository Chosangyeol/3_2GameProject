using UnityEngine;

public class State_BossChase : IState
{
    readonly EnemyBase enemy;
    readonly StateMachine fsm;
    readonly int patternCount;
    public int patternIndex;

    public State_BossChase(EnemyBase enemy, StateMachine fsm, int patternCount)
    {
        this.enemy = enemy;
        this.fsm = fsm;
        this.patternCount = patternCount;
    }

    public void OnEnter()
    {
        patternIndex = Random.Range(0, patternCount);
        enemy.anim.SetBool("isMoving", true);
    }

    public void Tick()
    {
        float dist = Vector3.Distance(enemy.transform.position, enemy.player.position);

        if (dist <= enemy.enemySO.attackRange)
        {
            fsm.ChangeState(new State_BossAttack(enemy, fsm, patternCount, patternIndex));
        }
        else
        {
            enemy.agent.SetDestination(enemy.player.position);
        }
        Debug.Log("Chase");
    }

    public void FixedTick() { }
    public void OnExit()
    {
        enemy.agent.ResetPath();
    }
}
