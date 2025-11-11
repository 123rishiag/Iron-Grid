using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WaveDetails
{
    public int basicEnemy;
    public int fastEnemy;
}

public class EnemyManager : MonoBehaviour
{
    [Header("Wave Details")]
    [SerializeField] private WaveDetails currentWave;
    [Space]
    [Header("Spawn Details")]
    [SerializeField] private Transform respawnTransform;
    [SerializeField] private float spawnCooldown = 3f;
    [Space]
    [Header("Enemy Prefabs")]
    [SerializeField] private GameObject basicEnemy;
    [SerializeField] private GameObject fastEnemy;

    private List<GameObject> enemiesToCreate;

    private float spawnTimer;

    private void Start()
    {
        enemiesToCreate = NewEnemyWave();
    }

    private void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0 && enemiesToCreate.Count > 0)
        {
            CreateEnemy();
            spawnTimer = spawnCooldown;
        }
    }

    private void CreateEnemy()
    {
        GameObject randomEnemy = GetRandomEnemy();
        GameObject newEnemy = Instantiate(randomEnemy, respawnTransform.position, Quaternion.identity);
    }

    private GameObject GetRandomEnemy()
    {
        int randomIndex = UnityEngine.Random.Range(0, enemiesToCreate.Count);
        GameObject choosenEnemy = enemiesToCreate[randomIndex];

        enemiesToCreate.Remove(choosenEnemy);

        return choosenEnemy;
    }

    private List<GameObject> NewEnemyWave()
    {
        List<GameObject> newEnemyList = new List<GameObject>();

        for (int i = 0; i < currentWave.basicEnemy; ++i)
        {
            newEnemyList.Add(basicEnemy);
        }
        for (int i = 0; i < currentWave.fastEnemy; ++i)
        {
            newEnemyList.Add(fastEnemy);
        }

        return newEnemyList;
    }
}
