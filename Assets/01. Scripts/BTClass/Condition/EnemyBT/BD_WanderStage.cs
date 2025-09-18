using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class BD_WanderStage : Action
{
    public float radius = 10f; // 반경
    public float wait = 1f; // 대기 시간
    private NavMeshAgent agent;
    private float nextMoveTime;

    public override void OnStart()
    {
        base.OnStart();
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();
        PickNewPoint();
    }

    public override TaskStatus OnUpdate()
    {
        if (Time.time < nextMoveTime) return TaskStatus.Running;
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            nextMoveTime = Time.time + wait;
            PickNewPoint();
        }
        return TaskStatus.Running;
    }

    void PickNewPoint()
    {
        Vector3 random = Random.insideUnitSphere * radius + transform.position;
        if (NavMesh.SamplePosition(random, out NavMeshHit hit, radius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }
}
