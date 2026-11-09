using System.Collections;
using UnityEngine;

public class PromQueenBoss : MonoBehaviour
{
    [SerializeField] private vidajefe bossHealth;
    [SerializeField] private float bossDamage = 15f;
    [SerializeField] private float bossStrength = 7f;

    [Header("Ataques Fase 1")]
    public float attackInterval = 2.5f;
    private float attackTimer;
    public Transform attackSpawnPoint;
    public GameObject flowerPrefab;

    [Header("Fase 2 (Prom queen 2)")]
    [SerializeField] private GameObject promQueenPhase2Prefab; // Arrastra aquí el Prefab del enemigo de la Fase 2
    private bool phase2Triggered = false;

    [Header("Detección y Objetivo")]
    [SerializeField] private Transform player;
    public Transform currentTarget;
    public float detectionRange = 10f;
    public bool isFacingRight = false;

    private float maxHealth;

    void Start()
    {
        attackTimer = attackInterval;

        if (bossHealth == null)
            bossHealth = GetComponent<vidajefe>();

        if (bossHealth != null)
            maxHealth = bossHealth.maxhealth;

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

        CheckPhaseTransition();
        FaceCurrentTarget();

        // Lógica de ataque de la Fase 1 (Solo flores)
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0)
        {
            ExecutePhase1Attack();
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

    void CheckPhaseTransition()
    {
        if (bossHealth == null || phase2Triggered) return;

        float healthPercentage = (bossHealth.health / maxHealth) * 100f;

        // Al llegar al 50% de vida, se destruye y despierta a la Fase 2
        if (healthPercentage <= 50f)
        {
            phase2Triggered = true;
            SpawnPhase2();
        }
    }

    void SpawnPhase2()
    {
        Debug.Log("¡Transformación! Desactivando Fase 1 y spawneando Fase 2...");

        if (promQueenPhase2Prefab != null)
        {
            Instantiate(promQueenPhase2Prefab, transform.position, transform.rotation);
        }

        Destroy(gameObject); // O gameObject.SetActive(false);
    }

    void ExecutePhase1Attack()
    {
        if (flowerPrefab != null && attackSpawnPoint != null)
        {
            GameObject spawnedAttack = Instantiate(flowerPrefab, attackSpawnPoint.position, Quaternion.identity);

            if (currentTarget != null)
            {
                Vector2 shootDirection = (currentTarget.position - attackSpawnPoint.position).normalized;
                Rigidbody2D rbProj = spawnedAttack.GetComponent<Rigidbody2D>();
                if (rbProj != null)
                {
                    rbProj.linearVelocity = shootDirection * 6f;
                }
            }

            EnemyProjectile projScript = spawnedAttack.GetComponent<EnemyProjectile>();
            if (projScript != null)
            {
                projScript.SetStrength(bossStrength);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController targetPlayer = collision.gameObject.GetComponent<PlayerController>();
        if (targetPlayer != null)
        {
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
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}