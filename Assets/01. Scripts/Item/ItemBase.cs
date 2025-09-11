using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class ItemBase : ScriptableObject
{
    public string itemName;
    public string itemDis;
    public int itemRarity; // 0 = �Ϲ�, 1 = ���, 2 = ���, 3 = ����, 4 = ����
}
