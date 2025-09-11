using UnityEngine;

public class C_StatBase
{
    [Header("Base Stats")]
    public int level = 1;
    public float maxHp = 100f;
    public float damage = 10f;
    public float defense = 2f;
    public float moveSpeed = 5f;
    public float rotateSpeed = 2000f;
    public float critChance = 0.1f;
    public float critMultiplier = 1.5f;

    [HideInInspector] public float hp;

    public void InitRuntime()
    {
        hp = maxHp;
    }
}
