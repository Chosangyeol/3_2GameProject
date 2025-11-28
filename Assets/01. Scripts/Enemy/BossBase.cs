using System.Collections;
using UnityEngine;

public class BossBase : EnemyBase
{
    public int patternCount;
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnEnable()
    {
        Reset();
        fsm.ChangeState(new State_BossIdle(this, fsm, patternCount));
    }

    protected override void Die()
    {
        TryBossDrop(enemySO.itemDropTable);
        // 보스 전용 포탈 오픈
        PoolManager.Instance.Push(this);
    }

    public override void StartAttack(int patternIndex = 0)
    {
        attackBehavior.ExecuteAttack(this, patternIndex);
        StartCoroutine(Wait(3f));
    }

    private void TryBossDrop(DropTableSO dropTable)
    {
        Debug.Log("보스 아이템 드랍");
    }

    //임시
    private IEnumerator Wait(float seconds)
    {
        Debug.Log("대기");
        yield return new WaitForSeconds(seconds);
        fsm.ChangeState(new State_BossChase(this, fsm, patternCount));
    }
}
