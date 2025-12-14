using UnityEngine;

public class Boss2Wall : MonoBehaviour
{
    public int count = 2;

    public void WallCountDown()
    {
        count--; 
        if (count <= 0)
            Destroy(gameObject);
    }
}
