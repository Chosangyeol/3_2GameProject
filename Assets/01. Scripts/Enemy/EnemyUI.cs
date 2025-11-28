using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    [SerializeField] private Slider hpSlider;
    [SerializeField] private EnemyBase target;
    [SerializeField] private Vector3 offset = new Vector3(0, 2f, 0);
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void OnEnable()
    {
        cam = Camera.main;
        target.hpChanged += UpdateHPBar;
    }

    private void OnDisable()
    {
        target.hpChanged -= UpdateHPBar;
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            gameObject.SetActive(false);
            return;
        }

        // 위치를 타겟 머리 위로
        transform.position = target.transform.position + offset;

        // 카메라를 향하게 (빌보드)
        transform.forward = cam.transform.forward;
    }

    private void UpdateHPBar(int current, int max)
    {
        hpSlider.maxValue = max;
        hpSlider.value = current;
    }
}
