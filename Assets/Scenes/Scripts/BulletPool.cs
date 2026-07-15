using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс BulletPool реализует паттерн Object Pool для пуль.
/// Вместо создания/удаления пуль при каждом выстреле, переиспользуем объекты из пула.
/// Это значительно оптимизирует производительность.
/// </summary>
public class BulletPool : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;     // префаб пули
    [SerializeField] private int initialSize = 20;        // количество пуль, создаваемых при старте

    private Queue<GameObject> availableBullets = new Queue<GameObject>(); // очередь свободных пуль

    /// <summary>
    /// Инициализация - создаём начальное количество пуль в пуле.
    /// Все пули изначально неактивны.
    /// </summary>
    private void Awake()
    {
        for (int i = 0; i < initialSize; i++)
        {
            GameObject bullet = CreateNewBullet();
            availableBullets.Enqueue(bullet);
        }
    }

    /// <summary>
    /// Создаёт новую пулю из префаба и добавляет её в пул.
    /// Пуля получает ссылку на этот пул для возврата после использования.
    /// </summary>
    /// <returns>Новая пуля (неактивная, в пуле)</returns>
    private GameObject CreateNewBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform);
        bullet.GetComponent<Bullet>().SetPool(this);
        bullet.SetActive(false);
        return bullet;
    }

    /// <summary>
    /// Берёт пулю из пула или создаёт новую, если пул пуст.
    /// Затем устанавливает позицию, ротацию и урон, и активирует пулю.
    /// </summary>
    /// <param name="position">Позиция для спауна пули</param>
    /// <param name="rotation">Ротация для спауна пули</param>
    /// <param name="damage">Урон, который будет наносить эта пуля</param>
    /// <returns>Активная пуля, готовая к использованию</returns>
    public GameObject GetBullet(Vector3 position, Quaternion rotation, float damage = 10f)
    {
        GameObject bullet;

        // Берём пулю из пула или создаём новую
        if (availableBullets.Count > 0)
            bullet = availableBullets.Dequeue();
        else
            bullet = CreateNewBullet();

        // Устанавливаем позицию и ротацию
        bullet.transform.SetPositionAndRotation(position, rotation);
        
        // Устанавливаем урон
        bullet.GetComponent<Bullet>().damage = damage;
        
        // Активируем пулю
        bullet.SetActive(true);
        return bullet;
    }

    /// <summary>
    /// Пуля вызывает этот метод, когда заканчивается либо время жизни, либо она попадает во врага.
    /// Пуля деактивируется и возвращается в пул для переиспользования.
    /// </summary>
    /// <param name="bullet">Пуля, которую нужно вернуть в пул</param>
    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        availableBullets.Enqueue(bullet);
    }
}
