using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New EnemySO", menuName = "EnemySO")]
public class EnemySO : ScriptableObject
{
    public string enemyName;
    public float maxHp;    
    public Enums.EnemyType enemyType;
    public float damage;
    public float attackRange;
    public float detectRange;
    public float moveSpeed;
    public float attackSpeed;

    [Header("아이템 드랍 테이블")]
    public DropTableSO itemDropTable;
    [Range(0f, 100f)]
    public float itemDropPersent;
}
