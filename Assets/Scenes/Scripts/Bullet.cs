using UnityEngine;

/// <summary>
/// Класс Bullet представляет пулю, выпущенную из оружия вертолёта.
/// Пули летят вверх и уничтожают врагов при попадании.
/// Используются из пула (BulletPool) для оптимизации памяти.
/// </summary>
public class Bullet : MonoBehaviour
{
    [SerializeField] private float lifeTime = 2f;   // время жизни пули (секунды)
    [SerializeField] private float speed = 15f;     // скорость полёта пули

    public float damage = 10f; // урон, наносимый врагу (устанавливается извне при выстреле)

    private float timer;       // счётчик оставшегося времени жизни
    private BulletPool pool;   // ссылка на пул, из которого была взята эта пуля

    /// <summary>
    /// Интерфейс для объектов, которые могут получать урон.
    /// Используется для проверки попадания пули в врага.
    /// </summary>
    public interface IDamageable
    {
        void TakeDamage(float amount);
    }

    /// <summary>
    /// Устанавливает ссылку на пул, чтобы пуля могла вернуться в пул после использования.
    /// </summary>
    /// <param name="ownerPool">Пул, который выдал эту пулю</param>
    public void SetPool(BulletPool ownerPool)
    {
        pool = ownerPool;
    }

    /// <summary>
    /// OnEnable вызывается при активации пули (её вытягивают из пула).
    /// Сбрасываем таймер жизни пули.
    /// </summary>
    private void OnEnable()
    {
        timer = lifeTime;
    }

    /// <summary>
    /// Каждый кадр: двигаем пулю вверх, уменьшаем таймер, проверяем время жизни.
    /// </summary>
    private void Update()
    {
        // Летим вверх
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        // Уменьшаем время жизни
        timer -= Time.deltaTime;
        if (timer <= 0f)
            ReturnToPool(); // пуля "состарилась" - возвращаем в пул
    }

    /// <summary>
    /// Проверяем коллизию с врагом.
    /// Если попали во врага - наносим урон и возвращаем пулю в пул.
    /// </summary>
    /// <param name="other">Коллайдер объекта, с которым произошла коллизия</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Пытаемся получить компонент IDamageable
            IDamageable damageable = other.GetComponent<IDamageable>();
            if (damageable != null)
            {
                // Наносим урон врагу
                damageable.TakeDamage(damage);
            }

            // Возвращаем пулю в пул
            ReturnToPool();
        }
    }

    /// <summary>
    /// Возвращает пулю в пул для переиспользования или деактивирует её.
    /// </summary>
    private void ReturnToPool()
    {
        if (pool != null)
            pool.ReturnBullet(gameObject);
        else
            gameObject.SetActive(false);
    }
}
