using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.UI;
using static UnityEngine.UI.GridLayoutGroup;

[CreateAssetMenu(fileName = "New WeaponMod", menuName = "WeaponMod/Bow2_1")]
public class Bow2_1Mod : WeaponModSO
{
    public int shotCount;
    public float delay;
    public float secondShotDamage = 0.7f;

    public override void OnFire(C_Weapon weapon, ref List<GameObject> projectiles, float speed)
    {
        C_Model owner = weapon.owner;
        owner.StartCoroutine(FireMultiShots(owner, weapon, projectiles[0],speed));
    }

    private IEnumerator FireMultiShots(C_Model owenr, C_Weapon weapon, GameObject baseProj, float speed)
    {
        yield return new WaitForSeconds(delay);

        if (weapon.attackBehavior is BowAttackBehavior bow)
        {
            PoolableMono proj = PoolManager.Instance.Pop(bow.projectile.name);
            proj.transform.position = owenr.transform.position;
            proj.transform.rotation = owenr.transform.rotation;

            if (proj.TryGetComponent(out PlayerProjectile pd))
            {
                pd.damage *= secondShotDamage;
            }

            Rigidbody rb = proj.GetComponent<Rigidbody>();
            if (rb != null)
                rb.linearVelocity = proj.transform.forward * speed;

        }
    }
}
