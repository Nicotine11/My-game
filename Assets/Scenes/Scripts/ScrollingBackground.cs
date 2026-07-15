using UnityEngine;

public class CityScroller : MonoBehaviour
{
    [Header("Настройки движения")]
    [SerializeField] private float scrollSpeed = 0.15f;     // Скорость опускания фона
    [SerializeField] private float endPositionY = -20f;  // Конечная позиция (когда остановиться)
    /*
    [Header("Настройки конца уровня")]
    [SerializeField] private GameManager gameManager;    // Ссылка на менеджер уровня
    [SerializeField] private string levelEndMethod = "OnLevelEnd"; // Метод для вызова
    */
    private bool isScrolling = true;
    private float startY;

    void Start()
    {
        startY = transform.position.y;

        // Если конечная позиция не задана, вычисляем её автоматически
        if (endPositionY == 0)
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                // Фон должен опуститься на свою высоту минус высота экрана
                float spriteHeight = sr.bounds.size.y;
                float cameraHeight = Camera.main.orthographicSize * 2;
                endPositionY = startY - (spriteHeight - cameraHeight);
            }
        }
    }

    void Update()
    {
        if (!isScrolling) return;

        // Двигаем фон вниз
        transform.Translate(Vector3.down * scrollSpeed * Time.deltaTime);

        // Проверяем, достигли ли конца
        if (transform.position.y <= endPositionY)
        {
            StopScrolling();
        }
    }
    
    void StopScrolling()
    {
        isScrolling = false;
        Debug.Log("Уровень пройден! Конец города.");

        // Вызываем событие окончания уровня
       /* if (gameManager != null)
        {
            gameManager.SendMessage(levelEndMethod, SendMessageOptions.DontRequireReceiver);
        }*/
    }
}