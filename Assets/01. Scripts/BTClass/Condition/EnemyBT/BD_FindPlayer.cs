using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class BD_FindPlayer : Conditional
{
    [Header("Blackboard")]
    public SharedGameObject target;     // 공유 타겟
    public SharedFloat detectRange;     // 기본 탐지 반경

    [Header("Layer/Filter")]
    public LayerMask playerLayer;  // Player 레이어 지정
    public bool useTriggers = true;     // 트리거 콜라이더도 감지할지

    [Header("Hysteresis / Memory")]
    public float buffer = 1.5f;         // 나갈 때 여유 반경(깜빡임 방지)
    public float forgetDelay = 0.6f;    // 이 시간 이상 범위 밖이면 target을 null로

    // 내부 상태
    private float _outSince = -1f;

    public override TaskStatus OnUpdate()
    {
        // 1) 현재 반경 내에서 "새로" 타겟을 찾는다 (target이 비었거나, 기존 타겟이 너무 멀어진 경우)
        float enterRadius = Mathf.Max(0f, detectRange.Value);
        float exitRadius = enterRadius + Mathf.Max(0f, buffer);

        // 이미 타겟이 있는지
        var cur = target.Value;

        // 가장 가까운 후보를 스캔
        var cols = Physics.OverlapSphere(
            transform.position,
            cur == null ? enterRadius : exitRadius,         // 새 타겟은 enter, 유지 판단은 exit
            playerLayer,
            useTriggers ? QueryTriggerInteraction.Collide : QueryTriggerInteraction.Ignore
        );

        // 후보 중 가장 가까운 놈 고르기
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

        // 2) 진입/유지 판정
        if (nearest != null)
        {
            // 탐지 반경(enter/exit) 안에 뭔가 있다 → 타겟을 최신으로 고정하고 성공
            target.Value = nearest;
            _outSince = -1f;
            // Debug.Log("플레이어 감지/유지");
            return TaskStatus.Success;
        }

        // 여기까지 오면 반경 안에 아무도 없음
        if (cur == null)
        {
            // 원래도 타겟이 없었다 → 실패 (Wander로 폴백)
            // Debug.Log("플레이어 없음");
            return TaskStatus.Failure;
        }

        // 타겟이 있었는데 지금은 반경 밖: forgetDelay 만큼 유예 후 놓침 처리
        if (_outSince < 0f) _outSince = Time.time;
        if (Time.time - _outSince >= forgetDelay)
        {
            target.Value = null;   // 완전히 놓침
            _outSince = -1f;
            // Debug.Log("플레이어 놓침 → target=null");
            return TaskStatus.Failure;
        }

        // 아직 유예 중이면 Success를 유지해서 급격히 흔들리지 않도록 함
        return TaskStatus.Success;
    }

    // 에디터에서 반경 시각화
    public override void OnDrawGizmos()
    {
        if (detectRange == null) return;
        Gizmos.color = new Color(1f, 0f, 0f, 0.35f);
        Gizmos.DrawWireSphere(transform.position, detectRange.Value);
        Gizmos.color = new Color(1f, 0.5f, 0f, 0.25f);
        Gizmos.DrawWireSphere(transform.position, detectRange.Value + Mathf.Max(0f, buffer));
    }
}
