using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossBase : EnemyBase
{
    public int patternCount;
    public bool specialTrigger = false;
    public bool wasSpecial = false;

    public Slider bossHpBar;
    public TMP_Text bossName;

    protected override void Awake()
    {
        base.Awake();
        bossName.text = enemySO.enemyName;
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
    }

    private void TryBossDrop(DropTableSO dropTable)
    {
        Debug.Log("보스 아이템 드랍");
    }

    public override IEnumerator AttackDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (!specialTrigger)
        {
            isAttack = false;
            fsm.ChangeState(new State_BossChase(this, fsm, patternCount));
        }
        else
        {
            isAttack = false;
        }
    }
}
