using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class C_Interactor : MonoBehaviour
{
    [Header("Scan")]
    [SerializeField] private float scanRadius = 3.0f;
    [SerializeField] private LayerMask interactableMask; // 상호작용 대상 레이어
    [SerializeField] private Transform eye;              // 카메라/머리 기준

    [Header("UI")]
    [SerializeField] private InteractUI promptUI;

    private IInteractable current;
    private readonly Collider[] _hits = new Collider[20];

    void Reset()
    {
        eye = Camera.main ? Camera.main.transform : transform;
    }

    void Update()
    {
        UpdateDetection();
    }

    public void TryInteract()
    {
        if (current == null) return;

        current.Interact(transform);
    }

    private void UpdateDetection()
    {
        int count = Physics.OverlapSphereNonAlloc(transform.position, scanRadius, _hits, interactableMask);
        IInteractable best = null;
        float bestScore = float.MinValue;

        for (int i = 0; i < count; i++)
        {
            var col = _hits[i];
            if (!col) continue;

            IInteractable it = col.GetComponentInParent<IInteractable>();
            if (it == null) continue;

            // 점수: 우선순위 + 화면 중앙과의 정렬
            Vector3 dir = (it.InteractTransform.position - eye.position).normalized;
            float dot = Vector3.Dot(eye.forward, dir); // 1에 가까울수록 화면 중앙
            float score = it.Priority * 10f + dot;

            if (score > bestScore)
            {
                bestScore = score;
                best = it;
            }
        }

        if (best != current)
        {
            if (current != null) current.OnUnfocus(transform);
            current = best;
            if (current != null) current.OnFocus(transform);
        }

        // UI
        if (current != null) promptUI?.Show(current.InteractName, current.InteractTransform.position);
        else promptUI?.Hide();
    }
}
