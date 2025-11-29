using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New EnemySO", menuName = "EnemySO")]
public class EnemySO : ScriptableObject
{
    public string enemyName;
    public int maxHp;    
    public Enums.EnemyType enemyType;
    public int damage;
    public float attackRange;
    public float detectRange;
    public float moveSpeed;
    public float attackSpeed;
    public int dropMoneyMin;
    public int dropMoneyMax;

    [Header("아이템 드랍 테이블")]
    public DropTableSO itemDropTable;
    [Range(0f, 100f)]
    public float itemDropPersent;
}
