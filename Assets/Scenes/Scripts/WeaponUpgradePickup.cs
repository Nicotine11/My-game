using UnityEngine;

public class WeaponUpgradePickup : MonoBehaviour
{
    [Header("Настройки бонуса")]
    [SerializeField] private GameObject pickupVFX;   // необязательно, эффект при подборе
    [SerializeField] private AudioClip pickupSound;  // необязательно, звук

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Ищем WeaponController на самом объекте ИЛИ на родителе
        // (на случай, если коллайдер вертолёта висит на дочернем объекте)
        WeaponController weapon = other.GetComponent<WeaponController>();
        if (weapon == null)
            weapon = other.GetComponentInParent<WeaponController>();

        if (weapon == null) return; // это не вертолёт - игнорируем

        weapon.UpgradeLevel();

        if (pickupVFX != null)
            Instantiate(pickupVFX, transform.position, Quaternion.identity);

        if (pickupSound != null)
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);

        Destroy(gameObject);
    }
}