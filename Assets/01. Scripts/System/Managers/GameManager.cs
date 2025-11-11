using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PoolableMono testEnemy;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Slash))
        {
            PoolableMono test = PoolManager.Instance.Pop(testEnemy.name);
            test.transform.position = new Vector3(10, 0, 10);
        }
    }
}
