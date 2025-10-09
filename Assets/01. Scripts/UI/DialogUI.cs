using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogUI : MonoBehaviour
{
    [SerializeField] private TMP_Text npcNameText;
    [SerializeField] private TMP_Text dialogText;

    [Header("UI ±¸¼º")]
    public Transform buttonParent;
    public GameObject buttonPrefab;

    private List<GameObject> currentButtons = new List<GameObject>();

    public void CreateButtons(List<NpcButton> buttons)
    {
        ClearButton();

        foreach (var button in buttons)
        {
            GameObject btnObj = Instantiate(buttonPrefab, buttonParent);
            Button btn = btnObj.GetComponent<Button>();
            TMPro.TextMeshProUGUI txt = btnObj.GetComponentInChildren<TMPro.TextMeshProUGUI>();

            txt.text = button.buttonText;

            btn.onClick.AddListener(() =>
            {
                button.onClickEvent?.Invoke();
            });
            currentButtons.Add(btnObj);
        }
    }

    public void ClearButton()
    {
        foreach (GameObject btnObj in currentButtons)
        {
            Destroy(btnObj);
        }
        currentButtons.Clear();
    }

    public void SetDialog(NpcSO npcData)
    {
        npcNameText.text = npcData.npcName;
        if (npcData.dialog.Length > 0)
        {
            dialogText.text = string.Join("\n", npcData.dialog[0].sentences);
        }
    }
}
