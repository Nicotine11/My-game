using UnityEngine;

[System.Serializable]
public class DropEntry
{
    public GameObject prefab;
    [Range(0f, 1f)] public float dropChance = 0.2f;
}

[CreateAssetMenu(fileName = "DropTable", menuName = "Weapons/Drop Table")]
public class DropTable : ScriptableObject
{
    public DropEntry[] entries;

    public void TrySpawnDrop(Vector3 position)
    {
        foreach (var entry in entries)
        {
            if (entry.prefab == null) continue;

            if (Random.value <= entry.dropChance)
            {
                Instantiate(entry.prefab, position, Quaternion.identity);
                return; // роняем максимум один бонус за врага - уберите return, если хотите несколько сразу
            }
        }
    }
}


/*
using UnityEngine;

[System.Serializable]
public class DropEntry
{
    public GameObject prefab;
    [Range(0f, 1f)] public float dropChance = 0.2f; // 0.2 = 20%
}

[CreateAssetMenu(fileName = "DropTable", menuName = "Weapons/Drop Table")]
public class DropTable : ScriptableObject
{
    public DropEntry[] entries;

    public void TrySpawnDrop(Vector3 position)
    {
        foreach (var entry in entries)
        {
            if (entry.prefab == null) continue;

            if (Random.value <= entry.dropChance)
            {
                Instantiate(entry.prefab, position, Quaternion.identity);
                return; // роняем максимум один бонус за врага - уберите return, если хотите несколько сразу
            }
        }
    }
}
*/