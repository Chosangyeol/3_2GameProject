using Unity.Cinemachine;
using UnityEngine;

public class CameraSciprt : MonoBehaviour
{
    public static CameraSciprt cam;

    private void Awake()
    {
        if (cam != null && cam != this)
        {
            Destroy(gameObject);
            return;
        }

        cam = this;
        DontDestroyOnLoad(gameObject);
    }
}
