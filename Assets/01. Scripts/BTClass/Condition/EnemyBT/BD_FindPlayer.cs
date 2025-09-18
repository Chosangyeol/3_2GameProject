using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class BD_FindPlayer : Conditional
{
    [Header("Blackboard")]
    public SharedGameObject target;     // ���� Ÿ��
    public SharedFloat detectRange;     // �⺻ Ž�� �ݰ�

    [Header("Layer/Filter")]
    public LayerMask playerLayer;  // Player ���̾� ����
    public bool useTriggers = true;     // Ʈ���� �ݶ��̴��� ��������

    [Header("Hysteresis / Memory")]
    public float buffer = 1.5f;         // ���� �� ���� �ݰ�(������ ����)
    public float forgetDelay = 0.6f;    // �� �ð� �̻� ���� ���̸� target�� null��

    // ���� ����
    private float _outSince = -1f;

    public override TaskStatus OnUpdate()
    {
        // 1) ���� �ݰ� ������ "����" Ÿ���� ã�´� (target�� ����ų�, ���� Ÿ���� �ʹ� �־��� ���)
        float enterRadius = Mathf.Max(0f, detectRange.Value);
        float exitRadius = enterRadius + Mathf.Max(0f, buffer);

        // �̹� Ÿ���� �ִ���
        var cur = target.Value;

        // ���� ����� �ĺ��� ��ĵ
        var cols = Physics.OverlapSphere(
            transform.position,
            cur == null ? enterRadius : exitRadius,         // �� Ÿ���� enter, ���� �Ǵ��� exit
            playerLayer,
            useTriggers ? QueryTriggerInteraction.Collide : QueryTriggerInteraction.Ignore
        );

        // �ĺ� �� ���� ����� �� ����
        GameObject nearest = null;
        float bestSqr = float.MaxValue;
        Vector3 selfPos = transform.position;
        for (int i = 0; i < cols.Length; i++)
        {
            var go = cols[i].attachedRigidbody ? cols[i].attachedRigidbody.gameObject : cols[i].gameObject;
            float sq = (go.transform.position - selfPos).sqrMagnitude;
            if (sq < bestSqr)
            {
                bestSqr = sq;
                nearest = go;
            }
        }

        // 2) ����/���� ����
        if (nearest != null)
        {
            // Ž�� �ݰ�(enter/exit) �ȿ� ���� �ִ� �� Ÿ���� �ֽ����� �����ϰ� ����
            target.Value = nearest;
            _outSince = -1f;
            // Debug.Log("�÷��̾� ����/����");
            return TaskStatus.Success;
        }

        // ������� ���� �ݰ� �ȿ� �ƹ��� ����
        if (cur == null)
        {
            // ������ Ÿ���� ������ �� ���� (Wander�� ����)
            // Debug.Log("�÷��̾� ����");
            return TaskStatus.Failure;
        }

        // Ÿ���� �־��µ� ������ �ݰ� ��: forgetDelay ��ŭ ���� �� ��ħ ó��
        if (_outSince < 0f) _outSince = Time.time;
        if (Time.time - _outSince >= forgetDelay)
        {
            target.Value = null;   // ������ ��ħ
            _outSince = -1f;
            // Debug.Log("�÷��̾� ��ħ �� target=null");
            return TaskStatus.Failure;
        }

        // ���� ���� ���̸� Success�� �����ؼ� �ް��� ��鸮�� �ʵ��� ��
        return TaskStatus.Success;
    }

    // �����Ϳ��� �ݰ� �ð�ȭ
    public override void OnDrawGizmos()
    {
        if (detectRange == null) return;
        Gizmos.color = new Color(1f, 0f, 0f, 0.35f);
        Gizmos.DrawWireSphere(transform.position, detectRange.Value);
        Gizmos.color = new Color(1f, 0.5f, 0f, 0.25f);
        Gizmos.DrawWireSphere(transform.position, detectRange.Value + Mathf.Max(0f, buffer));
    }
}
