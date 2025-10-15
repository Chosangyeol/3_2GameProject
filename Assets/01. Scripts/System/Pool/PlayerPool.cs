using UnityEngine;

public class PlayerPool : MonoBehaviour
{
    public PoolingListSO playerProjectiles;

    private void Awake()
    {
        CreatePool();
    }

    private void CreatePool()
    {
        PoolManager.Instance = new PoolManager(transform);
        playerProjectiles.PoolList.ForEach(p =>
        {
            PoolManager.Instance.CreatePool(p.Prefab, p.Count);
        });
    }
}
