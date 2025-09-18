using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class BD_TargetInRange : Conditional
{
    public SharedGameObject target;
    public SharedFloat range;

    public override TaskStatus OnUpdate()
    {
        if (target.Value == null) return TaskStatus.Failure;
        return Vector3.Distance(transform.position, target.Value.transform.position) <= range.Value ? TaskStatus.Success : TaskStatus.Failure;
    }
}
