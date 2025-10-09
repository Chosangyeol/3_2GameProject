using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class ItemBaseSO : ScriptableObject
{
    public string itemName;
    public string itemDis;
    public int itemRarity; // 0 = 일반, 1 = 고급, 2 = 희귀, 3 = 전설, 4 = 유물
}
