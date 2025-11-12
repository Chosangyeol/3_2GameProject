using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private EnemySpawner instance;
    public static EnemySpawner Instance => Instance;

    private void Awake()
    {
        instance = this;
    }

    public List<EnemyWave> waves;
    private int nowWave = 0;
    private int enemyInWaveNum = 0;
    private int currentDieEnemyNum;
    private PoolManager pool;

    public GameObject spawnEffect;
    private Queue<GameObject> spawnEffects = new Queue<GameObject>();


    private void Start()
    {
        pool = PoolManager.Instance;
        StartWave();
    }

    private void OnEnable()
    {
        EnemyBase.OnEnemyDie += CheckWaveEnd;
    }
    public void StartWave()
    {
        StartCoroutine(WaveSpawn(waves[nowWave]));
    }

    private IEnumerator WaveSpawn(EnemyWave waves)
    {
        // Enemy 생성 위치 표시
        foreach (var enemy in waves.enemyInfo)
        {
            GameObject effect = Instantiate(spawnEffect, enemy.spawnPos.transform);
            spawnEffects.Enqueue(effect);
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(2f);
        
        // 잠시 대기 이후 스폰

        foreach (var enemy in waves.enemyInfo)
        {
            SpawnEnemy(enemy);
            yield return new WaitForSeconds(0.3f);
        }
        if (nowWave < this.waves.Count)
        {
            nowWave++;
        }
    }

    private void CheckWaveEnd(EnemyBase enemy)
    {
        currentDieEnemyNum++;
        if (currentDieEnemyNum == enemyInWaveNum)
        {
            if (nowWave < waves.Count)
            {
                enemyInWaveNum = 0;
                currentDieEnemyNum = 0;
                StartWave();
            }
            else
            {
                Debug.Log("웨이브 종료");
            }
        }
    }

    private void SpawnEnemy(EnemyInfo enemy)
    {
        //Enemy 생성 시 이펙트 추가 예정
        Destroy(spawnEffects.Dequeue());
        PoolableMono spawnEnemy = pool.Pop(enemy.enemyPrefab.name);
        enemyInWaveNum++;
        spawnEnemy.transform.position = enemy.spawnPos.position;
    }

    //private IEnumerator DestroyEffect(GameObject effect)
    //{
    //    yield return new WaitForSeconds(2f);
    //    Destroy(effect);

    //}

    
}

[System.Serializable]
public class EnemyWave
{
    public List<EnemyInfo> enemyInfo;
}

[System.Serializable]
public class EnemyInfo
{
    public Transform spawnPos;
    public PoolableMono enemyPrefab;
}
