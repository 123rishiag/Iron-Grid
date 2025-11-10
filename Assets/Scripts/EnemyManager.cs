using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Transform respawnTransform;
    [SerializeField] private float spawnCooldown = 3f;

    [Header("Enemy Prefabs")]
    [SerializeField] private GameObject basicEnemy;

    private float spawnTimer;

    private void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0)
        {
            CreateEnemy();
            spawnTimer = spawnCooldown;
        }
    }

    private void CreateEnemy()
    {
        GameObject newEnemy = Instantiate(basicEnemy, respawnTransform.position, Quaternion.identity);
    }
}
