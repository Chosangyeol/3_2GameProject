using Player;
using UnityEngine;

public class StaffAttackBehavior : IPlayerAttackBe
{
    public GameObject projectile;
    public float speed;

    public StaffAttackBehavior(GameObject projectile)
    {
        this.projectile = projectile;
        this.speed = projectile.GetComponent<PlayerProjectile>().speed;
    }

    public void Execute(C_Model attacker, C_Weapon weapon)
    {
        Debug.Log("스테프 일반 공격");
    }
}
