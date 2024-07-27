using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallistaObjectScript : MonoBehaviour
{
    public Transform cannonBallPrefab;
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    private GameObject activeBullet;
    private List<GameObject> targets = new List<GameObject>();
    private float timeTillNextShot = 0f;
    private GameObject activeTarget;

    public float bulletSpeed;
    public float fireRate = 2f; // Mermi ateşleme aralığı

    void Update()
    {
        if (activeBullet != null)
        {
            activeBullet.transform.position += activeBullet.transform.forward * bulletSpeed * Time.deltaTime;
        }

        if (timeTillNextShot > 0)
        {
            timeTillNextShot -= Time.deltaTime;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("monster") && !targets.Contains(other.gameObject))
        {
            targets.Add(other.gameObject);
            if (activeTarget == null)
            {
                activeTarget = other.gameObject; // Hedef ilk giren monster olur
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("monster") && targets.Contains(other.gameObject))
        {
            targets.Remove(other.gameObject);
            if (activeTarget == other.gameObject)
            {
                // Eğer çıkarılan hedef aktif hedef ise yeni bir hedef seç
                activeTarget = GetNextTarget();
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("monster"))
        {
            if (activeTarget == null || !targets.Contains(activeTarget))
            {
                activeTarget = other.gameObject; // Eğer aktif hedef geçersizse mevcut hedefi ayarla
            }

            LookAtMonster();
            if (timeTillNextShot <= 0)
            {
                BulletFire();
                timeTillNextShot = fireRate;
            }
        }
    }

    private GameObject GetNextTarget()
    {
        if (targets.Count > 0)
        {
            GameObject target = targets[0];
            targets.Remove(target);
            return target;
        }
        return null;
    }

    private void LookAtMonster()
    {
        if (activeTarget != null)
        {
            Vector3 direction = (activeTarget.transform.position - cannonBallPrefab.position).normalized;
            cannonBallPrefab.forward = direction;
        }
    }

    public void BulletFire()
    {
        if (bulletPrefab != null && activeTarget != null)
        {
            activeBullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            Destroy(activeBullet, 2f); // Mermiyi 2 saniye sonra yok et
        }
    }
}
