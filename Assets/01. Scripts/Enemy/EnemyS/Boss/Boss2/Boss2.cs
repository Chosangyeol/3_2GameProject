using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using Player;
using System.Collections;
using UnityEngine;

public class Boss2 : BossBase
{
    private LineRenderer lr;
    public LineRenderer Lr => lr;


    [Header("패턴 1 - 돌진")]
    public bool canRush = true;
    private float timer = 0;
    private float rushCool = 10f;

    [Header("패턴 2 - 점프 낙석")]
    public PoolableMono boss2JumpWarning;
    public PoolableMono boss2Stone;

    [Header("패턴 3 - 부채꼴 공격")]
    public PoolableMono boss2Pattern3Warning;

    [Header("패턴 4 - 휠윈드")]
    public GameObject boss2Pattern4Warning;

    [Header("특수 패턴")]
    public GameObject specialWalls;
    public Transform boss2Center;
    public GameObject specialWarning;
    public bool specialOn = false;
    private bool specialReady = false;



    protected override void Awake()
    {
        base.Awake();
        lr = GetComponent<LineRenderer>();

        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        lr.positionCount = 2;

        lr.enabled = false; 
        attackBehavior = new Boss2Attack();
        boss2Center = GameObject.FindGameObjectWithTag("Boss2Center").transform;
        specialWalls = GameObject.FindGameObjectWithTag("Boss2Walls");
        specialWarning = GameObject.FindGameObjectWithTag("Boss2SpecialWarning");
        specialWalls.SetActive(false);

    }

    protected override void Update()
    {
        if (!canRush)
        {
            timer += Time.deltaTime;
            if (timer >= rushCool)
            {
                canRush = true;
                timer = 0;
            }
        }

        if (specialReady && !isAttack)
            StartCoroutine(SpecialDamaged());

        if (specialTrigger && !wasSpecial && !isAttack)
            StartCoroutine(Special());

        fsm.Tick();
    }

    public override void TakeDamage(int amount)
    {
        Stat.curHp -= amount;
        damagedEffect.Play();

        if (Stat.curHp <= Stat.maxHp/2)
        {
            specialTrigger = true;
            Debug.Log("특수패턴 사용 가능");
        }

        if (Stat.curHp <= 0)
        {
            Die();
        }
    }

    IEnumerator Special()
    {
        wasSpecial = true;
        fsm.ChangeState(new State_BossSpecial(this, fsm, patternCount));
        agent.enabled = false;

        yield return MoveToCenter(boss2Center.position);

        var player = FindAnyObjectByType<C_Model>();
        var cc = player.GetComponent<CharacterController>();

        cc.enabled = false;
        player.transform.position =
            boss2Center.position + new Vector3(-5, 0, 5);
        cc.enabled = true;

        yield return WallUp();
        specialOn = true;

        specialTrigger = false;
        yield return StartCoroutine(AttackDelay(2f));

        StartCoroutine(SpecialWarning());
        canRush = true;
        timer = 0;

        agent.enabled = true;
        specialTrigger = false;

    }

    IEnumerator MoveToCenter(Vector3 center)
    {

        Vector3 startPos = transform.position;

        if (Physics.Raycast(
        center + Vector3.up * 20f,
        Vector3.down,
        out RaycastHit hit,
        50f,
        LayerMask.GetMask("Ground")))
        {
            center = hit.point;
        }

        Vector3 midPos = (startPos + center) * 0.5f;
        midPos.y += 8f;
        float dur = 1.0f;
        float t = 0f;


        while (t < 1f)
        {
            t += Time.deltaTime / dur;
            transform.position = Bezier(startPos, midPos, center, t);

            // 회전 고정 (X,Z 기울기 방지)
            Vector3 euler = transform.eulerAngles;
            transform.rotation = Quaternion.Euler(0f, euler.y, 0f);

            yield return null;
        }

        transform.position = center;

        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator WallUp()
    {
        specialWalls.SetActive(true);

        float t = 0f;

        while (t < 2f)
        {
            t += Time.deltaTime;
            specialWalls.transform.position += Vector3.up * 5f * Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator SpecialWarning()
    {
        specialWarning.SetActive(true);

        float t = 0f;
        
        bool rush = false;

        while (t <= 30f)
        {
            t += Time.deltaTime;
            Vector3 scale = specialWarning.transform.localScale;
            scale.x += Time.deltaTime;
            scale.z += Time.deltaTime;
            specialWarning.transform.localScale = scale;

            if (t >= 15f && !rush)
            {
                canRush = true;
                timer = 0;
                rush = true;
            }

            yield return null;
        }

        specialReady = true;

        
    }

    IEnumerator SpecialDamaged()
    {
        specialTrigger = true;
        specialReady = false;

        fsm.ChangeState(new State_BossSpecial(this, fsm, patternCount));

        yield return MoveToCenter(boss2Center.position);

        yield return new WaitForSeconds(3f);

        specialWalls.SetActive(false);
        specialWarning.SetActive(false);

        Collider[] coll = Physics.OverlapSphere(boss2Center.position, 15f);

        foreach (Collider col in coll)
        {
            if (col.CompareTag("Player"))
            {
                col.GetComponentInChildren<C_Model>().Damaged(70);
            }
        }

        specialOn = false;
        specialTrigger = false;
        StartCoroutine(AttackDelay(2f));
    }


    Vector3 Bezier(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        return (1 - t) * (1 - t) * a
             + 2 * (1 - t) * t * b
             + t * t * c;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Vector3 left =
            Quaternion.Euler(0, -22.5f, 0) * transform.forward;
        Vector3 right =
            Quaternion.Euler(0, 22.5f, 0) * transform.forward;

        Gizmos.DrawRay(transform.position, left * 10f);
        Gizmos.DrawRay(transform.position, right * 10f);
    }
}
