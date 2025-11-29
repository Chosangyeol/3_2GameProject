using UnityEngine;

public class SetSpawnPos : MonoBehaviour
{
    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = transform.position;
        Debug.Log("Player spawn position set to: " + transform.position);
    }
}
