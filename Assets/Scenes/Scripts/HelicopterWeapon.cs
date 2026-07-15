using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Класс WeaponController управляет стрельбой вертолёта.
/// Поддерживает разные уровни оружия (WeaponLevelData), автоогонь и управление по Input System.
/// Стреляет из пула пуль (BulletPool) для оптимизации.
/// </summary>
public class WeaponController : MonoBehaviour
{
    public Transform firePoint;           // точка, из которой вылетают пули
    public BulletPool bulletPool;         // пул пуль для переиспользования

    [Tooltip("Уровни оружия по порядку, от слабого к сильному")]
    public WeaponLevelData[] levels;      // массив уровней оружия

    [Header("Автоогонь")]
    [SerializeField] private bool autoFire = true; // стреляет ли автоматически или по команде

    private int currentLevelIndex = 0;    // индекс текущего уровня оружия
    private float fireTimer = 0f;         // таймер до следующего выстрела
    private bool isFiring = false;        // флаг: идёт ли стрельба (для режима по кнопке)

    /// <summary>
    /// Свойство для быстрого доступа к текущим параметрам оружия.
    /// </summary>
    private WeaponLevelData CurrentLevel => levels[currentLevelIndex];

    /// <summary>
    /// Каждый кадр уменьшаем таймер и проверяем, нужно ли стрелять.
    /// </summary>
    private void Update()
    {
        fireTimer -= Time.deltaTime;

        // Стреляем если автоогонь включен ИЛИ если игрок держит кнопку атаки
        if (autoFire || isFiring)
        {
            TryShoot();
        }
    }

    /// <summary>
    /// Пробует произвести выстрел, если таймер готов и уровни оружия инициализированы.
    /// </summary>
    public void TryShoot()
    {
        if (fireTimer > 0f) return;                           // ещё не время стрелять
        if (levels == null || levels.Length == 0) return;    // защита от пустого массива уровней

        Shoot();
        fireTimer = CurrentLevel.fireRate; // устанавливаем таймер на следующий выстрел
    }

    /// <summary>
    /// Производит выстрел.
    /// Если пуль больше одной - разбрасывает их веером с углом spreadAngle.
    /// </summary>
    private void Shoot()
    {
        if (bulletPool == null || firePoint == null) return; // защита от пустых ссылок

        int count = CurrentLevel.bulletCount;         // сколько пуль выстрелить
        float dmg = CurrentLevel.damage;               // урон каждой пули

        // Если одна пуля - стреляем по центру
        if (count == 1)
        {
            bulletPool.GetBullet(firePoint.position, firePoint.rotation, dmg);
            return;
        }

        // Если несколько пуль - создаём веер
        // Вычисляем начальный угол так, чтобы пули были симметричны относительно центра
        float startAngle = -CurrentLevel.spreadAngle * (count - 1) / 2f;

        for (int i = 0; i < count; i++)
        {
            float angle = startAngle + CurrentLevel.spreadAngle * i;
            Quaternion rot = firePoint.rotation * Quaternion.Euler(0, 0, angle);
            bulletPool.GetBullet(firePoint.position, rot, dmg);
        }
    }

    /// <summary>
    /// Повышает уровень оружия на один (если не достигли максимума).
    /// </summary>
    public void UpgradeLevel()
    {
        if (currentLevelIndex < levels.Length - 1)
            currentLevelIndex++;
    }

    /// <summary>
    /// Устанавливает уровень оружия на конкретный индекс.
    /// Значение ограничивается диапазоном валидных индексов.
    /// </summary>
    /// <param name="index">Индекс уровня для установки</param>
    public void SetLevel(int index)
    {
        currentLevelIndex = Mathf.Clamp(index, 0, levels.Length - 1);
    }

    /// <summary>
    /// Обработчик Input System для действия "Attack" (атака).
    /// Используется если стрельба по нажатию кнопки, а не автоматическая.
    /// </summary>
    /// <param name="context">Контекст события Input System</param>
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
            isFiring = true;      // кнопка нажата
        else if (context.canceled)
            isFiring = false;     // кнопка отпущена
    }
}
