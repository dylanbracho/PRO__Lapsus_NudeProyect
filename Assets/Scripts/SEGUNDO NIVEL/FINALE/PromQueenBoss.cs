using UnityEngine;

public class PromQueenBoss : MonoBehaviour
{
    
    [SerializeField] private Health bossHealth; // Arrastra aquí el componente Health de la Prom Queen

    public float attackInterval = 2.5f;
    private float attackTimer;


    public Transform attackSpawnPoint;
    public GameObject flowerPrefab;     // Fase 1: Tormenta de flores
    public GameObject ribSlashPrefab;    // Fase 2 (50% HP): Ataque de costilla
    public GameObject implantPrefab;     // Fase 3 (25% HP): Implante explosivo

    public bool isVulnerable = false;
    public float vulnerabilityDuration = 3f;
    private float vulnerabilityTimer;

    private int currentPhase = 1;
    private float maxHealth;

    void Start()
    {
        attackTimer = attackInterval;

        if (bossHealth == null)
        {
            bossHealth = GetComponent<Health>();
        }

        if (bossHealth != null)
        {
            maxHealth = bossHealth.currentHealth;
        }

        Debug.Log("Prom Queen: ¿Qué diablos haces aquí?! (Combate iniciado)");
    }

    void Update()
    {
        if (bossHealth == null || bossHealth.dead) return;

        CheckPhaseTransition();

        if (isVulnerable)
        {
            // Ventana de vulnerabilidad (la jefa hace pose y se queda quieta)
            vulnerabilityTimer -= Time.deltaTime;
            if (vulnerabilityTimer <= 0)
            {
                isVulnerable = false;
                attackTimer = attackInterval;
                Debug.Log("Prom Queen sale de la pose y retoma el ataque.");
            }
        }
        else
        {
            // Ciclo normal de ataques
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {
                ExecutePhaseAttack();
                attackTimer = attackInterval;

                // Probabilidad de abrir su ventana de vulnerabilidad tras atacar
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

        // Fase 3: A 25% de vida (Implantes explosivos)
        if (healthPercentage <= 25f && currentPhase < 3)
        {
            currentPhase = 3;
            attackInterval = 1.8f; // Ataca más rápido
            Debug.Log("¡Fase 3 activada! Prom Queen lanza implantes explosivos.");
        }
        // Fase 2: A 50% de vida (Ataque de costilla)
        else if (healthPercentage <= 50f && currentPhase < 2)
        {
            currentPhase = 2;
            attackInterval = 2.1f;
            Debug.Log("¡Fase 2 activada! Prom Queen revela su forma monstruosa.");
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

            case 3:
                if (implantPrefab != null && attackSpawnPoint != null)
                    Instantiate(implantPrefab, attackSpawnPoint.position, Quaternion.identity);
                break;
        }
    }

    void StartVulnerabilityWindow()
    {
        isVulnerable = true;
        vulnerabilityTimer = vulnerabilityDuration;
        Debug.Log("¡La Prom Queen está vulnerable en su pose!");
    }

    // Método para que Sara le haga daño SOLO cuando está vulnerable
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isVulnerable && (collision.GetComponent<PlayerController>() != null || collision.name.Contains("Sara")))
        {
            if (bossHealth != null)
            {
                // Le resta vida usando el script Health que ya tienes[cite: 1]
                bossHealth.TakeDamage(15f);
                Debug.Log("¡Golpe exitoso a la Prom Queen!");
            }
        }
    }
}