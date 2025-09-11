using UnityEngine;

public class C_Health : MonoBehaviour
{
    public C_StatBase stats; // 캐릭터 스탯
    public bool isInvulnearable { get; private set; } // 무적인가?

    // 체력이 바뀔때 & 죽을때 델리게이트
    public System.Action<float, float> onHealthChanged;
    public System.Action onDied;

    private void Awake()
    {
        if (stats == null) stats = new C_StatBase();
        stats.InitRuntime();
        Notify();
    }

    public void SetInvulnerable(bool v) => isInvulnearable = v;

    public void TakeDamage(float amount)
    {
        if (isInvulnearable) return;
        amount = Mathf.Max(1f, amount - stats.defense);
        stats.hp -= amount;
        Notify();

        if (stats.hp <= 0f)
        {
            stats.hp = 0f;
            onDied?.Invoke();
            // 추후 죽음 처리 이후 로직 작성
        }
    }

    public void Heal(float amount)
    {
        stats.hp = Mathf.Min(stats.maxHp, stats.hp + amount);
        Notify();
    }

    void Notify() => onHealthChanged?.Invoke(stats.hp, stats.maxHp);
}
