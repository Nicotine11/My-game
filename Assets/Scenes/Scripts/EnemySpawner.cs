using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс EnemySpawner отвечает за создание врагов с определённым интервалом.
/// Использует паттерн Object Pool для оптимизации - переиспользует объекты врагов вместо создания новых.
/// Враги появляются в случайных точках из массива spawnPoints.
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    [Header("Что спавнить")]
    [SerializeField] private GameObject enemyPrefab; // префаб врага для создания

    [Header("Тайминг")]
    [SerializeField] private float spawnInterval = 2f;   // интервал между появлениями врагов (в секундах)
    [SerializeField] private int initialPoolSize = 10;    // количество врагов, создаваемых при старте

    [Header("Точки появления")]
    [SerializeField] private Transform[] spawnPoints;     // массив точек на сцене, где появляются враги

    private Queue<GameObject> availableEnemies = new Queue<GameObject>(); // очередь доступных врагов в пуле
    private float spawnTimer; // таймер для отслеживания времени между спавнами

    /// <summary>
    /// Инициализация - создаём пул врагов при загрузке сцены.
    /// Все враги изначально неактивны и готовы к использованию.
    /// </summary>
    private void Awake()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject enemy = CreateNewEnemy();
            availableEnemies.Enqueue(enemy);
        }
    }

    /// <summary>
    /// Каждый кадр проверяем таймер и спауним врагов с нужным интервалом.
    /// </summary>
    private void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            SpawnEnemy();
            spawnTimer = spawnInterval;
        }
    }

    /// <summary>
    /// Создаёт новый врага из префаба и добавляет его в пул.
    /// Враг при создании получает ссылку на этот спавнер.
    /// </summary>
    /// <returns>Новый объект врага (неактивный)</returns>
    private GameObject CreateNewEnemy()
    {
        GameObject enemy = Instantiate(enemyPrefab, transform);
        enemy.GetComponent<Enemy>().SetSpawner(this);
        enemy.SetActive(false);
        return enemy;
    }

    /// <summary>
    /// Спауним врага либо из пула (если есть), либо создаём нового.
    /// Позиция врага берётся из случайной точки из массива spawnPoints.
    /// </summary>
    private void SpawnEnemy()
    {
        if (spawnPoints == null || spawnPoints.Length == 0) return; // защита от пустого массива точек

        // Выбираем случайную точку появления
        Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Берём врага из пула или создаём нового
        GameObject enemy;
        if (availableEnemies.Count > 0)
            enemy = availableEnemies.Dequeue();
        else
            enemy = CreateNewEnemy();

        // Переводим врага в позицию и активируем
        enemy.transform.position = point.position;
        enemy.SetActive(true);
    }

    /// <summary>
    /// Враг вызывает этот метод, когда умирает.
    /// Враг возвращается в пул для переиспользования вместо уничтожения.
    /// </summary>
    /// <param name="enemy">Объект врага, который нужно вернуть в пул</param>
    public void ReturnEnemy(GameObject enemy)
    {
        enemy.SetActive(false);
        availableEnemies.Enqueue(enemy);
    }
}
