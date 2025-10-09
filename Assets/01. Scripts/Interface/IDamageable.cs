using UnityEngine;

public interface IDamageable
{
    void Damaged(float amount, GameObject attacker = null);
}
