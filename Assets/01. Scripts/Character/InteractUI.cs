using TMPro;
using UnityEngine;

public class InteractUI : MonoBehaviour
{
    [SerializeField] private Canvas canvas;  // World Space ĵ���� ����
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Vector3 worldOffset = new Vector3(0, 1.8f, 0);
    [SerializeField] private Transform cam;

    void Reset()
    {
        cam = Camera.main ? Camera.main.transform : null;
    }

    public void Show(string prompt, Vector3 worldPos)
    {
        if (!canvas) return;
        if (text) text.text = prompt;
        canvas.enabled = true;
        canvas.transform.position = worldPos + worldOffset;
        if (cam) canvas.transform.LookAt(canvas.transform.position + cam.forward); // ī�޶� ���ϰ�
    }

    public void Hide()
    {
        if (canvas) canvas.enabled = false;
    }
}
