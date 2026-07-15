using UnityEngine;

[CreateAssetMenu(fileName = "WeaponLevel", menuName = "Weapons/Weapon Level Data")]
public class WeaponLevelData : ScriptableObject
{
    [Header("Основное")]
    public string levelName = "Base";     // просто для удобства в инспекторе
    public int bulletCount = 1;           // сколько пуль за выстрел
    public float spreadAngle = 15f;       // угол между пулями (если их больше 1)

    [Header("Параметры")]
    public float fireRate = 0.3f;         // задержка между выстрелами
    public float damage = 10f;
}