using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int initialSize = 20;

    private Queue<GameObject> availableBullets = new Queue<GameObject>();

    private void Awake()
    {
        for (int i = 0; i < initialSize; i++)
        {
            GameObject bullet = CreateNewBullet();
            availableBullets.Enqueue(bullet);
        }
    }

    private GameObject CreateNewBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform);
        bullet.GetComponent<Bullet>().SetPool(this);
        bullet.SetActive(false);
        return bullet;
    }
    public GameObject GetBullet(Vector3 position, Quaternion rotation, float damage = 10f)
    {
        GameObject bullet;

        if (availableBullets.Count > 0)
            bullet = availableBullets.Dequeue();
        else
            bullet = CreateNewBullet();

        bullet.transform.SetPositionAndRotation(position, rotation);
        bullet.GetComponent<Bullet>().damage = damage;
        bullet.SetActive(true);
        return bullet;
    }
    /*Старый код
    public GameObject GetBullet(Vector3 position, Quaternion rotation)
    {
        GameObject bullet;

        if (availableBullets.Count > 0)
            bullet = availableBullets.Dequeue();
        else
            bullet = CreateNewBullet();

        bullet.transform.SetPositionAndRotation(position, rotation);
        bullet.SetActive(true);
        return bullet;
    }
    */
    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        availableBullets.Enqueue(bullet);
    }
}
