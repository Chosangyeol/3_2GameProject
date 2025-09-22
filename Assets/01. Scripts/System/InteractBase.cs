using UnityEngine;
using UnityEngine.Events;

public abstract class InteractBase : MonoBehaviour, IInteractable
{
    [Header("Interact 메뉴")]
    [SerializeField] protected string interactName = "F - 상호작용";
    [SerializeField] protected int priority = 0;
    [SerializeField] protected Transform interactTransform;

    [Header("interact 조건")]
    [SerializeField] protected bool requireFacing = false;
    [SerializeField, Range(0f, 180f)] protected float facingAngle = 45f;
    [SerializeField] protected float interactRange = 3f;
    [SerializeField] protected LayerMask blockerMask;

    [Header("이벤트")]
    public UnityEvent onFocused;
    public UnityEvent onUnfocused;
    public UnityEvent onInteracted;

    public string InteractName => interactName;
    public int Priority => priority;
    public Transform InteractTransform => interactTransform != null ? interactTransform : transform;

    protected virtual void Reset()
    {
        interactTransform = transform;
    }

    public virtual void OnFocus(Transform interactor)
    {
        onFocused?.Invoke();
    }

    public virtual void OnUnfocus(Transform interactor)
    {
        onUnfocused?.Invoke();
    }

    public virtual void Interact(Transform interactor)
    {
        onInteracted?.Invoke();
    }

    public bool IsValid(Transform interactor)
    {
        Vector3 toMe = (InteractTransform.position - interactor.position);
        if (toMe.sqrMagnitude > interactRange * interactRange) return false;

        if (requireFacing)
        {
            Vector3 forward = interactor.forward;
            float ang = Vector3.Angle(forward, toMe);
            if (ang > facingAngle) return false;
        }

        if (blockerMask.value != 0)
        {
            if (Physics.Raycast(interactor.position + Vector3.up * 1.5f, toMe.normalized, out RaycastHit hit, toMe.magnitude, blockerMask))
            {
                return false;
            }
        }
        return true;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(InteractTransform ? InteractTransform.position : transform.position, interactRange);

    }
#endif
}
