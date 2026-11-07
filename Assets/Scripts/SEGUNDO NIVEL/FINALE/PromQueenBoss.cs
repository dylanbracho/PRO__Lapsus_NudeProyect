using UnityEngine;

public class PromQueenBoss : MonoBehaviour
{
    [SerializeField] private Health bossHealth;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite phase1Sprite;
    [SerializeField] private Sprite phase2Sprite;

    public float attackInterval = 2.5f;
    private float attackTimer;

    public Transform attackSpawnPoint;
    public GameObject flowerPrefab;
    public GameObject ribSlashPrefab;

    public bool isVulnerable = false;
    public float vulnerabilityDuration = 3f;
    private float vulnerabilityTimer;

    private int currentPhase = 1;
    private float maxHealth;

    void Start()
    {
        attackTimer = attackInterval;

        if (bossHealth == null)
            bossHealth = GetComponent<Health>();

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (bossHealth != null)
            maxHealth = bossHealth.currentHealth;

        if (spriteRenderer != null && phase1Sprite != null)
            spriteRenderer.sprite = phase1Sprite;
    }

    void Update()
    {
        if (bossHealth == null || bossHealth.dead) return;

        CheckPhaseTransition();

        if (isVulnerable)
        {
            vulnerabilityTimer -= Time.deltaTime;
            if (vulnerabilityTimer <= 0)
            {
                isVulnerable = false;
                attackTimer = attackInterval;
            }
        }
        else
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {
                ExecutePhaseAttack();
                attackTimer = attackInterval;

                if (Random.value > 0.4f)
                {
                    StartVulnerabilityWindow();
                }
            }
        }
    }

    void CheckPhaseTransition()
    {
        float healthPercentage = (bossHealth.currentHealth / maxHealth) * 100f;

        if (healthPercentage <= 50f && currentPhase < 2)
        {
            currentPhase = 2;
            attackInterval = 2.0f;

            if (spriteRenderer != null && phase2Sprite != null)
                spriteRenderer.sprite = phase2Sprite;
        }
    }

    void ExecutePhaseAttack()
    {
        switch (currentPhase)
        {
            case 1:
                if (flowerPrefab != null && attackSpawnPoint != null)
                    Instantiate(flowerPrefab, attackSpawnPoint.position, Quaternion.identity);
                break;
            case 2:
                if (ribSlashPrefab != null && attackSpawnPoint != null)
                    Instantiate(ribSlashPrefab, attackSpawnPoint.position, Quaternion.identity);
                break;
        }
    }

    void StartVulnerabilityWindow()
    {
        isVulnerable = true;
        vulnerabilityTimer = vulnerabilityDuration;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isVulnerable && (collision.GetComponent<PlayerController>() != null || collision.name.Contains("Sara")))
        {
            if (bossHealth != null)
            {
                bossHealth.TakeDamage(15f);
            }
        }
    }
}