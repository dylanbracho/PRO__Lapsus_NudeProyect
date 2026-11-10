using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PQ2 : MonoBehaviour
{
    [SerializeField] private vidajefe bossHealth;
    [SerializeField] private Slider bossSliderFase2;

    [Header("Movimiento y Persecución")]
    public float moveSpeed = 3f;
    public float meeleAttackRange = 1.5f;

    [Header("Combat & Stats")]
    public float attackInterval = 1.8f;
    private float attackTimer;
    [SerializeField] private float bossDamage = 20f;
    [SerializeField] private float bossStrength = 9f;

    [Header("Ataque a Distancia (Implante Único)")]
    public Transform attackSpawnPoint;
    public GameObject implantPrefab;
    private bool hasShotImplant = false; // Bandera para que el implante se lance una sola vez si lo deseas

    [Header("Objetivo")]
    [SerializeField] private Transform player;
    public Transform currentTarget;
    public bool isFacingRight = false;

    private Rigidbody2D bossRb;
    private Animator animator;

    private void OnEnable()
    {
        if (bossHealth == null)
            bossHealth = GetComponent<vidajefe>();

        if (bossHealth != null)
        {
            bossHealth.health = bossHealth.maxhealth;

            if (bossSliderFase2 != null)
            {
                bossSliderFase2.maxValue = bossHealth.maxhealth;
                bossSliderFase2.value = bossHealth.health;
            }
        }

        hasShotImplant = false; // Reinicia el implante al reactivarse la fase
    }

    void Start()
    {
        bossRb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        attackTimer = attackInterval;

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

        FaceCurrentTarget();

        float distanceToPlayer = Vector2.Distance(transform.position, currentTarget.position);

        // Persecución: Se acerca a Sara si está fuera de rango de melé
        if (distanceToPlayer > meeleAttackRange)
        {
            Vector2 targetPos = new Vector2(currentTarget.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
        }

        // Opcional: Si quieres que lance el implante una vez al iniciar la fase 2
        /*
        if (!hasShotImplant)
        {
            hasShotImplant = true;
            ShootImplant();
        }
        */
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

    // Ataque a distancia (Implante)
    void ShootImplant()
    {
        if (implantPrefab != null && attackSpawnPoint != null)
        {
            if (animator != null) animator.SetTrigger("Attack2");

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

    // Ataque normal por colisión directa (Aquí es donde colisiona y triggerea la animación con Sara)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController targetPlayer = collision.gameObject.GetComponent<PlayerController>();
        if (targetPlayer != null)
        {
            // ¡Dispara la animación de ataque al colisionar con Sara!
            if (animator != null)
            {
                animator.SetTrigger("Attack");
            }

            targetPlayer.TakeDamage(bossDamage);
            targetPlayer.hitTime = 0.5f;
            targetPlayer.hitForce = bossStrength;
            targetPlayer.hitFromRight = (collision.transform.position.x <= transform.position.x);
        }
    }

    // Recibe daño de los ataques de Sara
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
}