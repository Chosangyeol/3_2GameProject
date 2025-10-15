using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyBase : PoolableMono
{
    public EnemySO enemySO;
    private EnemyStat Stat;

    [HideInInspector]
    public NavMeshAgent agent;
    [HideInInspector]
    public Rigidbody rb;
    [HideInInspector]
    public Animator anim;
    [HideInInspector]
    public float lastAttackTime;
    [HideInInspector]
    public Transform player;

    public UnityEvent<float, float> onHealthChanged;

    public IAttackBehavior attackBehavior;

    private StateMachine fsm;

    #region Unity Event
    public virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (enemySO != null && GetStat() == null)
        {
            Reset();
        }

        fsm = new StateMachine();
    }

    private void Start()
    {
        fsm.ChangeState(new State_Idle(this, fsm));

    }

    private void Update()
    {
        fsm.Tick();
    }

    private void FixedUpdate()
    {
        fsm.FixedTick();
    }

    public virtual void OnEnable()
    {

    }
    #endregion

    public override void Reset()
    {
        Stat = new EnemyStat(enemySO);

    }

    public EnemyStat GetStat()
    {
        return Stat;
    }

    public void StartAttack()
    {
        if (Time.time - lastAttackTime >= Stat.attackSpeed)
        {       
            attackBehavior.ExecuteAttack(this);
            lastAttackTime = Time.time;
        }
    }

    public void TakeDamage(float amount)
    {
        Stat.curHp -= amount;
        onHealthChanged?.Invoke(Stat.curHp, Stat.maxHp);
        if (Stat.curHp <= 0)
        {
            Die();
        }
    }
    
    public void Die()
    {
        //PoolManager.Instance.Push(this);
        Destroy(this.gameObject);
    }
}
