using UnityEngine;

public class C_StatBase
{
    [Header("Base Stats")]
    public float maxHp = 100f;
    public C_Weapon weapon;
    public float damage = 0f;
    public float defense = 2f;
    public float moveSpeed = 5f;
    public float attackSpeed = 0f;

    public int modingChance = 0;

    public float critChance = 0.1f;
    public float critMultiplier = 1.5f;



    public float rotateSpeed = 2000f;
    
    [HideInInspector] public float hp;

    public void InitRuntime()
    {
        hp = maxHp;
    }
}
