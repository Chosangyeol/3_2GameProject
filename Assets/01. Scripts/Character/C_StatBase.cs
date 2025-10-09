using UnityEngine;

public class C_StatBase
{
    public C_StatBaseSO statBaseSO;
    public float maxHp;
    public float curHp;
    public C_Weapon weapon;
    public float damage;
    public float moveSpeed;
    public float attackSpeed;

    public float money;

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
        this.weapon = new C_Weapon(Enums.WeaponType.None);
    }

    public void InitRuntime()
    {
        hp = maxHp;
    }
}
