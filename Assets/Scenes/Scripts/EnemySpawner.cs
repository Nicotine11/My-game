using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Что спавнить")]
    [SerializeField] private GameObject enemyPrefab;

    [Header("Тайминг")]
    [SerializeField] private float spawnInterval = 2f;   // раз в сколько секунд спавнить
    [SerializeField] private int initialPoolSize = 10;

    [Header("Точки появления")]
    [SerializeField] private Transform[] spawnPoints;     // пустые объекты по краям экрана

    private Queue<GameObject> availableEnemies = new Queue<GameObject>();
    private float spawnTimer;

    private void Awake()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject enemy = CreateNewEnemy();
            availableEnemies.Enqueue(enemy);
        }
    }

    private void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            SpawnEnemy();
            spawnTimer = spawnInterval;
        }
    }

    private GameObject CreateNewEnemy()
    {
        GameObject enemy = Instantiate(enemyPrefab, transform);
        enemy.GetComponent<Enemy>().SetSpawner(this);
        enemy.SetActive(false);
        return enemy;
    }

    private void SpawnEnemy()
    {
        if (spawnPoints == null || spawnPoints.Length == 0) return;

        Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject enemy;
        if (availableEnemies.Count > 0)
            enemy = availableEnemies.Dequeue();
        else
            enemy = CreateNewEnemy();

        enemy.transform.position = point.position;
        enemy.SetActive(true);
    }

    public void ReturnEnemy(GameObject enemy)
    {
        enemy.SetActive(false);
        availableEnemies.Enqueue(enemy);
    }
}