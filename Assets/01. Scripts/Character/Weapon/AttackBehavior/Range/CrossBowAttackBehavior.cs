using Player;
using UnityEngine;

public class CrossBowAttackBehavior : IPlayerAttackBe
{
    public GameObject projectile;
    public float speed;

    public CrossBowAttackBehavior(GameObject projectile)
    {
        this.projectile = projectile;
        this.speed = projectile.GetComponent<PlayerProjectile>().speed;
    }

    public void Execute(C_Model attacker, C_Weapon weapon)
    {
        Debug.Log("석궁 일반 공격");
    }
}
