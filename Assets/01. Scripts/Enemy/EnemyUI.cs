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

        if (target != null)
        {
            target.onHealthChanged.AddListener(UpdateHPBar);
            UpdateHPBar(target.GetStat().curHp, target.GetStat().maxHp);
        }
    }

    private void OnEnable()
    {
        UpdateHPBar(target.GetStat().curHp, target.GetStat().maxHp);
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

    private void UpdateHPBar(float current, float max)
    {
        float ratio = current / max;
        hpSlider.value = ratio;
    }
}
