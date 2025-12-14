using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Chase : IState
{
    readonly EnemyBase enemy;
    readonly StateMachine fsm;

    public State_Chase(EnemyBase enemy, StateMachine fsm)
    {
        this.enemy = enemy;
        this.fsm = fsm;
    }

    public void OnEnter()
    {
        enemy.anim.SetBool("Move", true);
    }

    public void Tick()
    {
        float dist = Vector3.Distance(enemy.transform.position, enemy.player.position);

        RotateToPlayer(enemy, 5f);

        if (dist <= enemy.enemySO.attackRange)
        {
            if (IsFacingPlayer(enemy, 5f))
            {
                fsm.ChangeState(new State_Attack(enemy, fsm));
            }
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
        enemy.anim.SetBool("Move", false);
    }

    private void RotateToPlayer(EnemyBase enemy, float rotSpeed)
    {
        Vector3 dir = (enemy.player.position - enemy.transform.position);
        dir.y = 0;

        Quaternion targetRot = Quaternion.LookRotation(dir);
        enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetRot, Time.deltaTime * rotSpeed);
    }

    private bool IsFacingPlayer(EnemyBase enemy, float thresholdAngle = 5f)
    {
        Vector3 dir = (enemy.player.position - enemy.transform.position).normalized;
        dir.y = 0;

        float angle = Vector3.Angle(enemy.transform.forward, dir);
        return angle < thresholdAngle;
    }
}
