using UnityEngine;

public class PromQueenBoss : MonoBehaviour
{
   
    public float maxHealth = 100f;
    public float currentHealth;

  
    public float attackInterval = 2.5f;
    private float attackTimer;


    public Transform attackSpawnPoint;
    public GameObject flowerPrefab;     // Fase 1: Tormenta de flores
    public GameObject ribSlashPrefab;    // Fase 2 (50% HP): Ataque de costilla
    public GameObject implantPrefab;     // Fase 3 (25% HP): Implante explosivo


    public bool isVulnerable = false;
    public float vulnerabilityDuration = 3f; // Tiempo que se queda quieta posando
    private float vulnerabilityTimer;

    private int currentPhase = 1;
    private bool introPlayed = false;

    void Start()
    {
        currentHealth = maxHealth;
        attackTimer = attackInterval;

        // La intro de la jefa al entrar a la escena
        IntroReaction();
    }

    void Update()
    {
        // Revisar cambios de fase por vida
        CheckPhaseTransition();

        if (isVulnerable)
        {
            // Ventana de vulnerabilidad (se queda quieta haciendo pose)
            vulnerabilityTimer -= Time.deltaTime;
            if (vulnerabilityTimer <= 0)
            {
                isVulnerable = false;
                attackTimer = attackInterval; // Vuelve a atacar
                Debug.Log("Prom Queen exits pose and resumes attack!");
            }
        }
        else
        {
            // Ciclo normal de ataque
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {
                ExecutePhaseAttack();
                attackTimer = attackInterval;

                // Opcional: cada cierto número de ataques, abre su ventana de pose/vulnerabilidad para que Sara la golpee
                // Aquí simulamos que tras un ataque fuerte entra en pose de diva
                if (Random.value > 0.5f)
                {
                    StartVulnerabilityWindow();
                }
            }
        }
    }

    void IntroReaction()
    {
        // Aquí puedes activar tu animación de "What the fuck are you doing here?" o UI flotante
        Debug.Log("Prom Queen: ¿Qué diablos haces aquí?! (Intro de combate iniciada)");
        introPlayed = true;
    }

    void CheckPhaseTransition()
    {
        float healthPercentage = (currentHealth / maxHealth) * 100f;

        // Fase 3: A 25% de vida (Desesperación / Implante)
        if (healthPercentage <= 25f && currentPhase < 3)
        {
            currentPhase = 3;
            Debug.Log("Fase 3: ¡Prom Queen muta y lanza implantes explosivos!");
            attackInterval = 1.8f; // Ataca más rápido
        }
        // Fase 2: A 50% de vida (Forma monstruosa / Costilla)
        else if (healthPercentage <= 50f && currentPhase < 2)
        {
            currentPhase = 2;
            Debug.Log("Fase 2: ¡Prom Queen revela su forma montruosa de cirugías (Rib attack)!");
            attackInterval = 2.1f;
        }
    }

    void ExecutePhaseAttack()
    {
        switch (currentPhase)
        {
            case 1:
                if (flowerPrefab != null && attackSpawnPoint != null)
                    Instantiate(flowerPrefab, attackSpawnPoint.position, Quaternion.identity);
                Debug.Log("Prom Queen lanza Tormenta de Flores.");
                break;

            case 2:
                if (ribSlashPrefab != null && attackSpawnPoint != null)
                    Instantiate(ribSlashPrefab, attackSpawnPoint.position, Quaternion.identity);
                Debug.Log("Prom Queen ataca con su costilla.");
                break;

            case 3:
                if (implantPrefab != null && attackSpawnPoint != null)
                    Instantiate(implantPrefab, attackSpawnPoint.position, Quaternion.identity);
                Debug.Log("Prom Queen lanza Implante Explosivo.");
                break;
        }
    }

    void StartVulnerabilityWindow()
    {
        isVulnerable = true;
        vulnerabilityTimer = vulnerabilityDuration;
        Debug.Log("Prom Queen strike a pose! ¡Ahora es vulnerable!");
    }

    // Método para que Sara le haga daño cuando la golpee en su pose
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Detecta si Sara la golpea durante la ventana de vulnerabilidad
        if (isVulnerable && (collision.GetComponent<PlayerController>() != null || collision.name.Contains("Sara")))
        {
            // O si choca con el ataque/espada de Sara (puedes ajustar esta condición según tu script de ataque de Sara)
            TakeDamage(25f); // Daño por golpe
        }
    }

    public void TakeDamage(float amount)
    {
        if (isVulnerable)
        {
            currentHealth -= amount;
            Debug.Log("¡Golpe crítico a la Prom Queen! Vida restante: " + currentHealth);

            if (currentHealth <= 0)
            {
                Die();
            }
        }
        else
        {
            Debug.Log("¡La Prom Queen es invulnerable o está atacando!");
        }
    }

    void Die()
    {
        Debug.Log("¡Prom Queen derrotada!");
        // Aquí activas la escena final / victoria
        gameObject.SetActive(false);
    }
}