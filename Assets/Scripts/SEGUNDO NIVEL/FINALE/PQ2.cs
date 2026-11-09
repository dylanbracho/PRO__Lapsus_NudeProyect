using UnityEngine;

public class PromQueenPhase2 : MonoBehaviour
{
    [SerializeField] private vidajefe bossHealth;

    [Header("Movimiento y Persecución")]
    public float moveSpeed = 2.5f;
    public float meeleAttackRange = 1.8f; // Distancia para hacer golpe directo
    public float detectionRange = 12f;

    [Header("Combat & Stats")]
    public float attackInterval = 1.8f;
    private float attackTimer;
    [SerializeField] private float bossDamage = 20f;
    [SerializeField] private float bossStrength = 9f;

    [Header("Ataque a Distancia")]
    public Transform attackSpawnPoint;
    public GameObject implantPrefab;

    [Header("Objetivo")]
    [SerializeField] private Transform player;
    public Transform currentTarget;
    public bool isFacingRight = false;

    private Rigidbody2D bossRb;
    private Animator animator;

    void Start()
    {
        bossRb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        attackTimer = attackInterval;

        if (bossHealth == null)
            bossHealth = GetComponent<vidajefe>();

        if (player == null)
        {
            PlayerController pc = FindFirstObjectByType<PlayerController>();
            if (pc != null) player = pc.transform;
        }

        currentTarget = player;
    }

    void Update()
    {
        if (bossHealth == null || bossHealth.health <= 0) return;

        if (currentTarget == null)
        {
            if (player != null) currentTarget = player;
            else return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, currentTarget.position);
        if (distanceToPlayer > detectionRange) return;

        FaceCurrentTarget();

        // Si está fuera del rango de melé, se va acercando a Sara
        if (distanceToPlayer > meeleAttackRange)
        {
            Vector2 targetPos = new Vector2(currentTarget.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
        }

        // Timer para ataques a distancia (implantes)
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0)
        {
            ShootImplant();
            attackTimer = attackInterval;
        }
    }

    void FaceCurrentTarget()
    {
        if (currentTarget != null)
        {
            float diffX = currentTarget.position.x - transform.position.x;
            if (!isFacingRight && diffX > 0f) Flip();
            else if (isFacingRight && diffX < 0f) Flip();
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    void ShootImplant()
    {
        if (implantPrefab != null && attackSpawnPoint != null)
        {
            GameObject spawnedAttack = Instantiate(implantPrefab, attackSpawnPoint.position, Quaternion.identity);

            if (currentTarget != null)
            {
                Vector2 shootDirection = (currentTarget.position - attackSpawnPoint.position).normalized;
                Rigidbody2D rbProj = spawnedAttack.GetComponent<Rigidbody2D>();
                if (rbProj != null)
                {
                    rbProj.linearVelocity = shootDirection * 7f;
                }
            }

            EnemyProjectile projScript = spawnedAttack.GetComponent<EnemyProjectile>();
            if (projScript != null)
            {
                projScript.SetStrength(bossStrength);
            }
        }
    }

    // Daño Melé por contacto físico directo (al acercarse a Sara)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController targetPlayer = collision.gameObject.GetComponent<PlayerController>();
        if (targetPlayer != null)
        {
            // Ejecutar animación de ataque melé cuando la tengas lista
            if (animator != null) animator.SetTrigger("Attack");

            targetPlayer.TakeDamage(bossDamage);
            targetPlayer.hitTime = 0.5f;
            targetPlayer.hitForce = bossStrength;
            targetPlayer.hitFromRight = (collision.transform.position.x <= transform.position.x);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<ProjectlieBehaviour>() != null || collision.name.Contains("Whip") || collision.name.Contains("Mazo"))
        {
            if (bossHealth != null)
            {
                bossHealth.RecibirDaño(15f);
                if (collision.GetComponent<ProjectlieBehaviour>() != null)
                {
                    Destroy(collision.gameObject);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, meeleAttackRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}