using UnityEngine;

/// <summary>
/// Класс DropEntry описывает один возможный дроп бонуса (оружие, щит и т.д.).
/// Содержит префаб и вероятность выпадения.
/// </summary>
[System.Serializable]
public class DropEntry
{
    public GameObject prefab;                              // префаб бонуса, который появляется
    [Range(0f, 1f)] public float dropChance = 0.2f;       // вероятность выпадения (0-1)
}

/// <summary>
/// Класс DropTable хранит таблицу возможных дропов и логику для случайного спауна.
/// Это ScriptableObject, который можно создать в редакторе и переиспользовать для разных врагов.
/// </summary>
[CreateAssetMenu(fileName = "DropTable", menuName = "Weapons/Drop Table")]
public class DropTable : ScriptableObject
{
    public DropEntry[] entries; // массив возможных дропов

    /// <summary>
    /// Пробует спаунить бонус на заданной позиции.
    /// Проходит по всем записям и выбирает первую, которая прошла проверку вероятности.
    /// Максимум спауним ОДИН бонус за врага (из-за return).
    /// </summary>
    /// <param name="position">Позиция, где спаунить бонус</param>
    public void TrySpawnDrop(Vector3 position)
    {
        foreach (var entry in entries)
        {
            if (entry.prefab == null) continue; // пропускаем пустые записи

            // Проверяем вероятность (Random.value возвращает число от 0 до 1)
            if (Random.value <= entry.dropChance)
            {
                // Спауним бонус и выходим (максимум один бонус за врага)
                Instantiate(entry.prefab, position, Quaternion.identity);
                return;
            }
        }
    }
}
