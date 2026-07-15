using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static Bullet;

public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Здоровье")]
    [SerializeField] private float maxHealth = 30f;
    private float currentHealth;

    [Header("Дроп бонусов")]
    [SerializeField] private DropTable dropTable; // ScriptableObject с шансами дропа

    private EnemySpawner spawner; // ссылка на спавнер, чтобы вернуться в пул

    public void SetSpawner(EnemySpawner ownerSpawner)
    {
        spawner = ownerSpawner;
    }

    private void OnEnable()
    {
        currentHealth = maxHealth; // на случай, если враг тоже будет из пула
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0f)
            Die();
    }

    private void Die()
    {
        dropTable?.TrySpawnDrop(transform.position);

        if (spawner != null)
            spawner.ReturnEnemy(gameObject);
        else
            Destroy(gameObject); // на случай если враг размещён на сцене вручную, без спавнера
    }

    // Если враг сталкивается с вертолётом напрямую (не пулей) - тоже можно нанести урон игроку
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // урон игроку - обсудим отдельно
        }
    }
}


/*Старый код
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Настройки врага")]
    [SerializeField] private int health = 1;           // Здоровье
    [SerializeField] private float scrollSpeed = 3f;   // Скорость движения вниз
    [SerializeField] private int scoreValue = 10;      // Очки за уничтожение

    void Update()
    {
        // Движение вниз
        transform.Translate(Vector3.down * scrollSpeed * Time.deltaTime);

        // Уничтожить, если улетел за экран
        if (transform.position.y < -6f)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Добавляем очки (позже реализуем менеджер счёта)
        Debug.Log($"Враг уничтожен! +{scoreValue} очков");

        // Эффект взрыва (опционально)
        // Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
*/