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

    [Header("Relevo a Fase 2 y UI")]
    [SerializeField] private GameObject promQueenPhase2Object; // El GameObject de la Fase 2 en escena
    [SerializeField] private GameObject uiFase1; // <-- Arrastra aquí 'uividajefe (1)'
    [SerializeField] private GameObject uiFase2; // <-- Arrastra aquí 'uividajefe (2)'
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

        CheckPhaseTransition();

        if (currentTarget == null)
        {
            if (player != null) currentTarget = player;
            else return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, currentTarget.position);
        if (distanceToPlayer > detectionRange) return;

        FaceCurrentTarget();

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

        if (maxHealth <= 0 && bossHealth.maxhealth > 0)
            maxHealth = bossHealth.maxhealth;

        if (bossHealth.health <= (maxHealth / 2f))
        {
            phase2Triggered = true;
            ActivatePhase2InScene();
        }
    }

    void ActivatePhase2InScene()
    {
        // 1. Activa y reposiciona la Fase 2
        if (promQueenPhase2Object != null)
        {
            promQueenPhase2Object.transform.position = transform.position;
            promQueenPhase2Object.SetActive(true);
        }

        // 2. Apaga la UI de la Fase 1 y enciende la UI de la Fase 2
        if (uiFase1 != null) uiFase1.SetActive(false);
        if (uiFase2 != null) uiFase2.SetActive(true);

        // 3. Desactiva la Fase 1
        gameObject.SetActive(false);
    }

    void ExecutePhase1Attack()
    {
        // Activa la animación de ataque si el Animator está asignado
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

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