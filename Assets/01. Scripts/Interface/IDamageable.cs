using UnityEngine;

public interface IDamageable
{
    void Damaged(int amount, GameObject attacker = null);
}
