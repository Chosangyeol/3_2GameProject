using UnityEngine;

public class PlayerProjectile : PoolableMono
{

    [HideInInspector]
    public float damage;
    public float speed;
    public float damageMultiplier = 1f;

    public override void Reset()
    {
        base.Reset();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Рћ РћСп / " + damage );
            other.GetComponent<EnemyBase>().TakeDamage(Mathf.RoundToInt(damage * damageMultiplier));
            PoolManager.Instance.Push(this);
        }

        if (other.CompareTag("Wall"))
        {
            PoolManager.Instance.Push(this);
        }
    }
}
