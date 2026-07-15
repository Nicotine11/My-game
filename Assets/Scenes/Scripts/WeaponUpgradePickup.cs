using UnityEngine;

/// <summary>
/// Класс WeaponUpgradePickup представляет бонус "Улучшение оружия".
/// Когда вертолёт касается этого бонуса, его оружие повышается на уровень.
/// При подборе может воспроизводиться VFX эффект и звук.
/// </summary>
public class WeaponUpgradePickup : MonoBehaviour
{
    [Header("Настройки бонуса")]
    [SerializeField] private GameObject pickupVFX;   // эффект при подборе (опционально)
    [SerializeField] private AudioClip pickupSound;  // звук при подборе (опционально)

    /// <summary>
    /// Проверяем коллизию с вертолётом.
    /// Если коснулись вертолёта - повышаем уровень оружия и удаляем бонус.
    /// </summary>
    /// <param name="other">Коллайдер объекта, который коснулся бонуса</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Ищем WeaponController на объекте или его родителе
        // (на случай если коллайдер вертолёта находится на дочернем объекте)
        WeaponController weapon = other.GetComponent<WeaponController>();
        if (weapon == null)
            weapon = other.GetComponentInParent<WeaponController>();

        if (weapon == null) return; // это не вертолёт - игнорируем

        // Повышаем уровень оружия
        weapon.UpgradeLevel();

        // Спауним VFX эффект если он задан
        if (pickupVFX != null)
            Instantiate(pickupVFX, transform.position, Quaternion.identity);

        // Проигрываем звук если он задан
        if (pickupSound != null)
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);

        // Удаляем бонус
        Destroy(gameObject);
    }
}
