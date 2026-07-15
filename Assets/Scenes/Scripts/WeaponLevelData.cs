using UnityEngine;

/// <summary>
/// Класс WeaponLevelData хранит параметры одного уровня оружия.
/// Это ScriptableObject, который можно создать и настроить в редакторе.
/// Разные уровни могут отличаться количеством пуль, их углом разброса, скоростью стрельбы и уроном.
/// </summary>
[CreateAssetMenu(fileName = "WeaponLevel", menuName = "Weapons/Weapon Level Data")]
public class WeaponLevelData : ScriptableObject
{
    [Header("Основное")]
    public string levelName = "Base";          // имя уровня (для удобства в инспекторе)
    public int bulletCount = 1;                 // сколько пуль выстреливается за раз
    public float spreadAngle = 15f;             // угол между пулями (если их больше 1)

    [Header("Параметры")]
    public float fireRate = 0.3f;               // задержка между выстрелами (в секундах)
    public float damage = 10f;                  // урон каждой пули
}
