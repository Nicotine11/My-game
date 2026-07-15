using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Класс HelicopterControl управляет движением вертолёта на основе входа игрока.
/// Вертолёт может двигаться в четырёх направлениях и ограничивается границами экрана.
/// Использует новую Input System для получения входа.
/// </summary>
public class HelicopterControl : MonoBehaviour
{
    [Header("Настройки движения")]
    [SerializeField] private float moveSpeed = 5f; // скорость движения вертолёта

    [Header("Границы экрана")]
    [SerializeField] private float leftBorder = -1f;    // левая граница движения
    [SerializeField] private float rightBorder = 1f;    // правая граница движения
    [SerializeField] private float topBorder = 6f;      // верхняя граница движения
    [SerializeField] private float bottomBorder = -6f;  // нижняя граница движения

    private Vector2 moveInput; // вектор входа (x, y) от Input System

    /// <summary>
    /// Каждый кадр обновляем позицию вертолёта на основе входа.
    /// Новая позиция ограничивается границами экрана.
    /// </summary>
    void Update()
    {
        // Вычисляем новую позицию: текущая + направление * скорость * время
        Vector3 newPosition = transform.position + new Vector3(moveInput.x, moveInput.y, 0) * moveSpeed * Time.deltaTime;

        // Ограничиваем позицию границами экрана
        newPosition.x = Mathf.Clamp(newPosition.x, leftBorder, rightBorder);
        newPosition.y = Mathf.Clamp(newPosition.y, bottomBorder, topBorder);

        // Применяем новую позицию
        transform.position = newPosition;
    }

    /// <summary>
    /// Метод-обработчик для действия Move из Input System.
    /// Вызывается автоматически при изменении входа (движение стика аналога или клавиш).
    /// Сохраняет текущий вектор движения в moveInput.
    /// </summary>
    /// <param name="context">Контекст события Input System с данными о движении</param>
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
}
