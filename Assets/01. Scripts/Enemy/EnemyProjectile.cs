using Player;
using UnityEngine;

public class EnemyProjectile : PoolableMono
{
    public EnemyBase owner;
    [HideInInspector]
    public int damage;
    [HideInInspector]
    public float timer;
    public float destroyTime;
    public float speed;

    public override void Reset()
    {
        base.Reset();
        timer = Time.time;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (Time.time - timer >= destroyTime)
        {
            PoolManager.Instance.Push(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<C_Model>().Damaged(damage);
            PoolManager.Instance.Push(this);
        }
    }
}
