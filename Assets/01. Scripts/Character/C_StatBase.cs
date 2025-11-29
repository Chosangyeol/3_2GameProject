using UnityEngine;

public class C_StatBase
{
    public C_StatBaseSO statBaseSO;
    public int maxHp;
    public int curHp;
    public int damage;
    public float moveSpeed;
    public float attackSpeed;

    public int money;

    public int modingChance;

    public float critChance;
    public float critMultiplier;


    public float rotateSpeed = 2000f;
    
    [HideInInspector] public float hp;

    public C_StatBase(C_StatBaseSO statBaseSO)
    {
        this.maxHp = statBaseSO.maxHp;
        this.curHp = maxHp;
        this.damage = 0;
        this.moveSpeed = statBaseSO.moveSpeed;
        this.attackSpeed = 0;
        this.money = 100;
        this.modingChance = 100;
        this.critChance = 0.1f;
        this.critMultiplier = 1.5f;
        this.rotateSpeed = 2000f;
    }

    public void AddMaxHp(int amount)
    {
        maxHp += amount;
        Heal(amount);
    }

    public void AddDamage(int amount)
    {
        damage += amount;
    }

    public void AddMoveSpeed(float amount)
    {
        moveSpeed += amount;
    }

    public void AddAttackSpeed(float amount)
    {
        attackSpeed += amount;
    }

    public void AddCritRate(float amount)
    {
        critChance += amount;
    }

    public void AddCritDamage(float amount)
    {
        critMultiplier += amount;
    }

    public void Heal(int amount)
    {
        curHp += amount;
        if (curHp > maxHp)
        {
            curHp = maxHp;
        }
    }

    public void InitRuntime()
    {
        hp = maxHp;
    }
}
