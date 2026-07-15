using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float lifeTime = 2f;
    [SerializeField] private float speed = 15f;

    public float damage = 10f; // будет устанавливаться извне при выстреле

    private float timer;
    private BulletPool pool;
    //Интерфейс для проверки попадания
    public interface IDamageable
    {
        void TakeDamage(float amount);
    }
    ///
    public void SetPool(BulletPool ownerPool)
    {
        pool = ownerPool;
    }
    //
    private void OnEnable()
    {
        timer = lifeTime;
    }
    //
    private void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        timer -= Time.deltaTime;
        if (timer <= 0f)
            ReturnToPool();
    }
    //
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            IDamageable damageable = other.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage); // damage - берём из WeaponLevelData, см. ниже
            }

            ReturnToPool();
        }
    }
    //
    private void ReturnToPool()
    {
        if (pool != null)
            pool.ReturnBullet(gameObject);
        else
            gameObject.SetActive(false);
    }
}






















/* Старый Код 
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Настройки пули")]
    [SerializeField] private float speed = 15f;    // Скорость полёта
    [SerializeField] private float lifetime = 3f;  // Жизнь пули (секунд)

    [Header("Урон")]
    [SerializeField] private int damage = 1;       // Урон по врагу

    void Start()
    {
        // Автоматически уничтожаем пулю через время, чтобы она не засоряла память
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Летим вверх (положительное направление по Y)
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }
    /*
    void OnTriggerEnter2D(Collider2D other)
    {
        // Проверяем, попали ли во врага
        if (other.CompareTag("Enemy"))
        {
            // Сообщаем врагу о попадании
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            // Уничтожаем пулю
            Destroy(gameObject);
        }
    }
} */