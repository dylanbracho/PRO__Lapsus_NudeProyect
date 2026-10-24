using UnityEngine;

public class GlitchedEnemy : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float detectionRange = 7f;
    [SerializeField] private float fireInterval = 1.5f;
    [SerializeField] private float projectileSpeed = 6f;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float enemyStrength = 7f;
    [SerializeField] public bool isFacingRight;
    [SerializeField] private float fireTimer = 0f;
    public float direction;

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            Vector2 targetPosition = new Vector2(player.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            fireTimer += Time.deltaTime;
            if (fireTimer >= fireInterval)
            {
                Shoot();
                fireTimer = 0f;
            }
        }
        else
        {
            fireTimer = 0f;
        }


        if (!isFacingRight && direction > 0f)
        {
            Flip();
        }
        else if (isFacingRight && direction < 0f)

        {
            Flip();
        }


    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;

        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;

    }



    void Shoot()
    {
        if (projectilePrefab != null && firePoint != null && player != null)
        {
            GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            Vector2 direction = (player.position - firePoint.position).normalized;

            Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = direction * projectileSpeed;
            }

            EnemyProjectile projectileScript = proj.GetComponent<EnemyProjectile>();
            if (projectileScript != null)
            {
                projectileScript.SetStrength(enemyStrength);
            }
        }
    }
}