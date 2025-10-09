using UnityEngine;

[CreateAssetMenu(fileName = "Player Stat", menuName = "SO/Data-Player")]
public class C_StatBaseSO : ScriptableObject
{
    public float maxHp;
    public float moveSpeed;
    public float critChance;
    public float critMultiplier;
}
