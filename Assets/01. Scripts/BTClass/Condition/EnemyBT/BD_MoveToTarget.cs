using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class BD_MoveToTarget : Action
{
    public SharedGameObject target;
    public SharedFloat stoppingDistance;
    private NavMeshAgent agent;

    public override void OnStart()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (target.Value != null)
        {
            agent.isStopped  = false;
            agent.stoppingDistance = stoppingDistance.Value;
            agent.SetDestination(target.Value.transform.position);
        }
    }

    public override TaskStatus OnUpdate()
    {
        if (target.Value == null) return TaskStatus.Failure;
        agent.SetDestination(target.Value.transform.position);
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            return TaskStatus.Success;
        return TaskStatus.Running;
    }
}
