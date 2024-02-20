using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float bulletSpeed = 10f;
    public float bulletLifetime = 2f;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;

    private float bulletTimer = 0f;
    private bool allEnemiesArrived = false;

    void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            touchPosition.z = 0f;

            RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                transform.position = touchPosition;
            }
        }

        bulletTimer += Time.deltaTime;
        if (bulletTimer >= 0.1f && allEnemiesArrived)
        {
            Shoot();
            bulletTimer = 0f;
        }

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Collider2D enemyCollider = enemy.GetComponent<Collider2D>();
            if (enemyCollider != null)
            {
                Collider2D[] bulletColliders = Physics2D.OverlapCircleAll(enemy.transform.position, 0.1f);
                foreach (Collider2D bulletCollider in bulletColliders)
                {
                    if (bulletCollider.CompareTag("Bullet"))
                    {
                        Destroy(bulletCollider.gameObject);
                    }
                }
            }
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.up * bulletSpeed;

        Destroy(bullet, bulletLifetime);
        SoundManager.instance.PlayBulletSound();
    }

    public void UpdateAllEnemiesArrived(bool allArrived)
    {
        allEnemiesArrived = allArrived;
    }
}