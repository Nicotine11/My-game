using UnityEngine;

/// <summary>
/// Класс CityScroller управляет движением фонового спрайта (города).
/// Фон медленно опускается вниз, создавая эффект прокрутки.
/// Когда фон достигает конца - движение останавливается (уровень пройден).
/// </summary>
public class CityScroller : MonoBehaviour
{
    [Header("Настройки движения")]
    [SerializeField] private float scrollSpeed = 0.15f;     // скорость движения фона вниз
    [SerializeField] private float endPositionY = -20f;     // Y позиция, где фон должен остановиться

    private bool isScrolling = true;  // идёт ли сейчас прокрутка
    private float startY;              // начальная Y позиция фона

    /// <summary>
    /// При загрузке сцены запоминаем начальную позицию фона.
    /// Если конечная позиция не установлена - вычисляем её автоматически
    /// на основе размера спрайта и камеры.
    /// </summary>
    void Start()
    {
        startY = transform.position.y;

        // Если конечная позиция не задана - вычисляем её
        if (endPositionY == 0)
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                // Вычисляем, на сколько должен опуститься фон
                float spriteHeight = sr.bounds.size.y;
                float cameraHeight = Camera.main.orthographicSize * 2;
                endPositionY = startY - (spriteHeight - cameraHeight);
            }
        }
    }

    /// <summary>
    /// Каждый кадр двигаем фон вниз, если прокрутка ещё идёт.
    /// Проверяем, достигла ли позиция конца.
    /// </summary>
    void Update()
    {
        if (!isScrolling) return; // прокрутка уже закончилась

        // Двигаем фон вниз
        transform.Translate(Vector3.down * scrollSpeed * Time.deltaTime);

        // Проверяем, достигли ли конца
        if (transform.position.y <= endPositionY)
        {
            StopScrolling();
        }
    }

    /// <summary>
    ///停止движение фона и вызываем событие конца уровня.
    /// </summary>
    void StopScrolling()
    {
        isScrolling = false;
        Debug.Log("Уровень пройден! Конец города.");

        // TODO: Здесь можно добавить логику конца уровня:
        // - перезагрузка сцены
        // - переход на следующий уровень
        // - показ экрана победы
    }
}
