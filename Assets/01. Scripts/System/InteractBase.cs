using UnityEngine;
using UnityEngine.Events;

public abstract class InteractBase : MonoBehaviour, IInteractable
{
    [Header("Interact �޴�")]
    [SerializeField] protected string interactName = "F - ��ȣ�ۿ�";
    [SerializeField] protected int priority = 0;
    [SerializeField] protected Transform interactTransform;

    [Header("�̺�Ʈ")]
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
}
