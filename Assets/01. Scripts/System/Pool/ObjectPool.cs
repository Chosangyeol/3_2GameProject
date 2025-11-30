using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public PoolingListSO playerProjectiles;
    public PoolingListSO playerEffects;
    public PoolingListSO playerItems;
    public PoolingListSO enemyPool;

    private void Awake()
    {
        playerProjectiles.PoolList.ForEach(p =>
        {
            PoolManager.Instance.CreatePool(p.Prefab, p.Count);
        });

        playerEffects.PoolList.ForEach(p =>
        {
            PoolManager.Instance.CreatePool(p.Prefab, p.Count);
        });

        playerItems.PoolList.ForEach(p =>
        {
            PoolManager.Instance.CreatePool(p.Prefab, p.Count);
        });

        enemyPool.PoolList.ForEach(p =>
        {
            PoolManager.Instance.CreatePool(p.Prefab, p.Count);
        });


    }
}
