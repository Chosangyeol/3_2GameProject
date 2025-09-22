using UnityEngine;

public interface IInteractable
{
    string InteractName { get; }
    int Priority { get; }
    Transform InteractTransform { get; }

    void OnFocus(Transform interactor);
    void OnUnfocus(Transform interactor);

    void Interact(Transform interactor);
}
