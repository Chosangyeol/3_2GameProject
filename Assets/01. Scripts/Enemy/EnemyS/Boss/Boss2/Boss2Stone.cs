using Player;
using System.Collections;
using UnityEngine;

public class Boss2Stone : PoolableMono
{
    public GameObject stone;

    public override void Reset()
    {
        base.Reset();
        stone.transform.position += Vector3.up * 30f;
        StartCoroutine(StoneDown());
    }

    private void Update()
    {
        
    }

    IEnumerator StoneDown()
    {
        yield return new WaitForSeconds(0.5f);

        float fallSpeed = 50f;

        while (true)
        {
            stone.transform.position += Vector3.down * fallSpeed * Time.deltaTime;

            if (Physics.Raycast(
                stone.transform.position,
                Vector3.down,
                out RaycastHit hit,
                1.5f,
                LayerMask.GetMask("Ground")))
            {
                stone.transform.position = hit.point;
                break;
            }


            yield return null;
        }

        Collider[] col = Physics.OverlapSphere(this.transform.position, 2f);

        foreach (Collider coll in col)
        {
            if (coll.CompareTag("Player"))
            {
                coll.GetComponentInChildren<C_Model>().Damaged(10);
            }
        }

        PoolManager.Instance.Push(this);
    }
}

