using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyOptions
{
    public GameObject enemyPrefab;
    public int poolSize = 10;
    public float spawnInterval = 1.0f;
    public float initialDelay = 5.0f;
    public float maxDistanceFromPlayer = 20f;
    public float maxDistanceCheckInterval = 0.5f;
    public int maxActiveEnemies = 5;

    [HideInInspector]
    public List<GameObject> pooledEnemies = new List<GameObject>();
    [HideInInspector]
    public int activeEnemyCount = 0;
    [HideInInspector]
    public float spawnTimer = 0.0f;
    [HideInInspector]
    public float maxDistanceCheckTimer = 0f;
}

public class EnemyPooler : MonoBehaviour
{
    public Transform minSpawn;
    public Transform maxSpawn;
    public List<EnemyOptions> enemyOptionsList;
    public float playerCheckInterval = 1.0f;

    private Transform target;
    private bool playerIsPresent;

    void Start()
    {
        target = PlayerHealthController.instance.transform;
        playerIsPresent = target.gameObject.activeSelf;

        if (minSpawn == null)
        {
            minSpawn = transform.Find("MinSpawn");
        }

        if (maxSpawn == null)
        {
            maxSpawn = transform.Find("MaxSpawn");
        }

        foreach (var enemyOption in enemyOptionsList)
        {
            StartCoroutine(SpawnEnemies(enemyOption)); // start the coroutine to spawn enemies for this type
        }

        StartCoroutine(CheckEnemiesDistance());
        StartCoroutine(CheckPlayerPresence()); // start the coroutine to check for player's presence
    }

    void Update()
    {
        // Update the positions of the minSpawn and maxSpawn to follow the player
        if (target != null && target.gameObject.activeSelf)
        {
            minSpawn.position = target.position + new Vector3(-11f, -7f, 0f);
            maxSpawn.position = target.position + new Vector3(11f, 7f, 0f);
            transform.position = target.position;
        }
    }

    IEnumerator SpawnEnemies(EnemyOptions enemyOptions)
    {
        yield return new WaitForSeconds(enemyOptions.initialDelay); // wait for initial delay before starting to spawn enemies

        // Create initial pool
        for (int i = 0; i < enemyOptions.poolSize; i++)
        {
            GameObject enemy = Instantiate(enemyOptions.enemyPrefab, SelectSpawnPoint(), Quaternion.identity);
            enemy.SetActive(false);
            enemyOptions.pooledEnemies.Add(enemy);
        }

        while (true)
        {
            enemyOptions.spawnTimer += Time.deltaTime;

            // Only spawn enemies if the player is present in the scene
            if (playerIsPresent && enemyOptions.spawnTimer >= enemyOptions.spawnInterval && enemyOptions.activeEnemyCount < enemyOptions.maxActiveEnemies)
            {
                enemyOptions.spawnTimer -= enemyOptions.spawnInterval;

                GameObject enemy = GetEnemy(enemyOptions);
                if (enemy != null)
                {
                    enemy.SetActive(true);
                    enemyOptions.activeEnemyCount++;
                }
            }

            yield return null;
        }
    }

    public Vector3 SelectSpawnPoint()
    {
        Vector3 spawnPoint = Vector3.zero;

        bool spawnVerticalEdge = Random.Range(0f, 1f) > .5f;

        if (spawnVerticalEdge)
        {
            spawnPoint.y = Random.Range(minSpawn.position.y, maxSpawn.position.y);

            if (Random.Range(0f, 1f) > .5f)
            {
                spawnPoint.x = maxSpawn.position.x;
            }             else
            {
                spawnPoint.x = minSpawn.position.x;
            }
        }
        else
        {
            spawnPoint.x = Random.Range(minSpawn.position.x, maxSpawn.position.x);

            if (Random.Range(0f, 1f) > .5f)
            {
                spawnPoint.y = maxSpawn.position.y;
            }
            else
            {
                spawnPoint.y = minSpawn.position.y;
            }
        }

        return spawnPoint;
    }

    public GameObject GetEnemy(EnemyOptions enemyOptions)
    {
        foreach (var enemy in enemyOptions.pooledEnemies)
        {
            if (!enemy.activeInHierarchy)
            {
                enemy.transform.position = SelectSpawnPoint();
                return enemy;
            }
        }

        return null;
    }


    public void DisableEnemy(GameObject enemy)
    {
        foreach (var enemyOption in enemyOptionsList)
        {
            if (enemyOption.pooledEnemies.Contains(enemy))
            {
                enemy.SetActive(false);
                enemyOption.activeEnemyCount--;
                break;
            }
        }
    }

    IEnumerator CheckEnemiesDistance()
    {
        while (true)
        {
            foreach (var enemyOption in enemyOptionsList)
            {
                enemyOption.maxDistanceCheckTimer += Time.deltaTime;

                if (enemyOption.maxDistanceCheckTimer >= enemyOption.maxDistanceCheckInterval)
                {
                    enemyOption.maxDistanceCheckTimer -= enemyOption.maxDistanceCheckInterval;

                    foreach (var enemy in enemyOption.pooledEnemies)
                    {
                        if (enemy.activeInHierarchy && Vector3.Distance(target.position, enemy.transform.position) > enemyOption.maxDistanceFromPlayer)
                        {
                            enemy.GetComponent<EnemyController>().DisableEnemy();
                        }
                    }
                }
            }

            yield return null;
        }
    }


    IEnumerator CheckPlayerPresence()
    {
        while (true)
        {
            yield return new WaitForSeconds(playerCheckInterval);
            playerIsPresent = target.gameObject.activeSelf;
        }
    }

    
}