using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static Bullet;

/// <summary>
/// Класс Enemy представляет врага в игре.
/// Враги могут получать урон, погибать и возвращаться в пул объектов для оптимизации памяти.
/// Реализует интерфейс IDamageable для получения урона от пуль.
/// </summary>
public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Здоровье")]
    [SerializeField] private float maxHealth = 30f; // максимальное здоровье врага
    private float currentHealth; // текущее здоровье врага

    [Header("Дроп бонусов")]
    [SerializeField] private DropTable dropTable; // ScriptableObject с таблицей бонусов (шансы дропа оружия)

    private EnemySpawner spawner; // ссылка на спавнер, чтобы вернуть врага в пул после смерти

    /// <summary>
    /// Устанавливает ссылку на спавнер, который создал этого врага.
    /// Нужна для возврата врага в пул вместо удаления.
    /// </summary>
    /// <param name="ownerSpawner">Спавнер, который управляет этим врагом</param>
    public void SetSpawner(EnemySpawner ownerSpawner)
    {
        spawner = ownerSpawner;
    }

    /// <summary>
    /// OnEnable вызывается при активации объекта (когда его вытягивают из пула).
    /// Восстанавливаем здоровье врага до максимума для переиспользования.
    /// </summary>
    private void OnEnable()
    {
        currentHealth = maxHealth; // на случай, если враг будет переиспользован из пула
    }

    /// <summary>
    /// Получить урон от пули (реализация интерфейса IDamageable).
    /// Проверяет, не убили ли врага, и если да - вызывает Die().
    /// </summary>
    /// <param name="amount">Количество урона, которое наносится врагу</param>
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0f)
            Die();
    }

    /// <summary>
    /// Враг погибает.
    /// Спауним бонус согласно DropTable, затем возвращаем врага в пул или удаляем.
    /// </summary>
    private void Die()
    {
        // Пробуем спаунить случайный бонус (оружие или что-то другое)
        dropTable?.TrySpawnDrop(transform.position);

        // Если враг был создан спавнером - возвращаем его в пул
        // Если враг был размещён вручную на сцене - удаляем его
        if (spawner != null)
            spawner.ReturnEnemy(gameObject);
        else
            Destroy(gameObject);
    }

    /// <summary>
    /// Коллизия с вертолётом (игроком) - враг может нанести урон игроку.
    /// Пока реализация не готова (комментарий: обсудим отдельно).
    /// </summary>
    /// <param name="other">Объект, с которым произошла коллизия</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // TODO: Реализовать урон игроку при столкновении с врагом
            // Здесь должен быть код для нанесения урона вертолёту
        }
    }
}
