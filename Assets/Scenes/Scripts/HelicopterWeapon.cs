using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponController : MonoBehaviour
{
    public Transform firePoint;
    public BulletPool bulletPool;

    [Tooltip("Уровни оружия по порядку, от слабого к сильному")]
    public WeaponLevelData[] levels;

    [Header("Автоогонь")]
    [SerializeField] private bool autoFire = true;

    private int currentLevelIndex = 0;
    private float fireTimer = 0f;
    private bool isFiring = false; // на случай, если стрельба по кнопке, а не авто

    private WeaponLevelData CurrentLevel => levels[currentLevelIndex];

    private void Update()
    {
        fireTimer -= Time.deltaTime;

        if (autoFire || isFiring)
        {
            TryShoot();
        }
    }

    public void TryShoot()
    {
        if (fireTimer > 0f) return;
        if (levels == null || levels.Length == 0) return; // защита от пустого массива

        Shoot();
        fireTimer = CurrentLevel.fireRate;
    }
    private void Shoot()
    {
        if (bulletPool == null || firePoint == null) return;

        int count = CurrentLevel.bulletCount;
        float dmg = CurrentLevel.damage; // у вас это поле уже есть в WeaponLevelData

        if (count == 1)
        {
            bulletPool.GetBullet(firePoint.position, firePoint.rotation, dmg);
            return;
        }

        float startAngle = -CurrentLevel.spreadAngle * (count - 1) / 2f;

        for (int i = 0; i < count; i++)
        {
            float angle = startAngle + CurrentLevel.spreadAngle * i;
            Quaternion rot = firePoint.rotation * Quaternion.Euler(0, 0, angle);
            bulletPool.GetBullet(firePoint.position, rot, dmg);
        }
    }
    public void UpgradeLevel()
    {
        if (currentLevelIndex < levels.Length - 1)
            currentLevelIndex++;
    }

    public void SetLevel(int index)
    {
        currentLevelIndex = Mathf.Clamp(index, 0, levels.Length - 1);
    }

    // Если стрельба по нажатию кнопки, а не авто - этот метод дергает Input System
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
            isFiring = true;
        else if (context.canceled)
            isFiring = false;
    }
}