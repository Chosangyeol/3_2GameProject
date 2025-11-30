using UnityEngine;

public class PlayerProjectile : PoolableMono
{

    [HideInInspector]
    public int damage;
    public float speed;
    public bool isReady = true;

    public bool isExplosive = false;
    public float explosiveRadius = 0f;

    public override void Reset()
    {
        base.Reset();
    }

    private void Update()
    {
        ColOn();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && !isExplosive)
        {
            Debug.Log("利 利吝 / " + damage );
            other.GetComponent<EnemyBase>().TakeDamage(damage);
            PoolManager.Instance.Push(this);
        }

        if (other.CompareTag("Enemy") && isExplosive)
        {
            Explosive();
            PoolManager.Instance.Push(this);
        }

        if (other.CompareTag("Wall"))
        {
            PoolManager.Instance.Push(this);
        }
    }

    private void Explosive()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, explosiveRadius);
        foreach (var col in cols)
        {
            EnemyBase enemy = col.GetComponent<EnemyBase>();
            if (enemy != null)
            {
                Debug.Log("利 利吝 / " + damage);
                enemy.TakeDamage(damage);
            }
        }
    }

    public void ColOn()
    {
        GetComponent<Collider>().enabled = isReady;
    }
}
