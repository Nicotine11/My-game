using UnityEngine;
using UnityEngine.InputSystem;

public class HelicopterControl : MonoBehaviour
{
    [Header("Настройки движения")]
    [SerializeField] private float moveSpeed = 5f;
    /*
    [Header("Настройки стрельбы")]
    [SerializeField] private GameObject bulletPrefab;    // Префаб пули
    [SerializeField] private Transform firePoint;        // Точка вылета пули
    [SerializeField] private BulletPool bulletPool;      // Пул пуль
    [SerializeField] private float fireRate = 0.15f;     // Выстрелов в секунду (0.15 = ~6-7 выстрелов в секунду)
    [SerializeField] private bool autoFire = true;       // Автоматическая стрельба
    
    [Tooltip("Уровни оружия по порядку, от слабого к сильному")]
    public WeaponLevelData[] levels;
    */
    //private int currentLevelIndex = 0;
    //private float fireTimer = 0f;

    
    [Header("Границы экрана")]
    [SerializeField] private float leftBorder = -1f;
    [SerializeField] private float rightBorder = 1f;
    [SerializeField] private float topBorder = 6f;
    [SerializeField] private float bottomBorder = -6f;


    private Vector2 moveInput;
    //private float nextFireTime;  // Когда можно стрелять снова

    /*void Start()
    {
        // Если точка выстрела не задана, используем центр вертолёта
        if (firePoint == null)
        {
            firePoint = transform;
        }
    }
    */
    void Update()
    {
        // Движение от InputSystem_Actions (Move возвращает Vector2)
        Vector3 newPosition = transform.position + new Vector3(moveInput.x, moveInput.y, 0) * moveSpeed * Time.deltaTime;

        // Ограничение границами
        newPosition.x = Mathf.Clamp(newPosition.x, leftBorder, rightBorder);
        newPosition.y = Mathf.Clamp(newPosition.y, bottomBorder, topBorder);

        transform.position = newPosition;
        /*
        // Стрельба (будем использовать позже)
        if (autoFire && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
        */
    }

    // Этот метод вызывается автоматически при действии Move (из InputSystem_Actions)
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    /*
    void TryShoot ()
    {
        if (bulletPool != null && firePoint != null)
        {
            bulletPool.GetBullet(firePoint.position, Quaternion.identity);
        }
    }
   */
}