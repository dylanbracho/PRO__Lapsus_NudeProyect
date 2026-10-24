using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private ParticleSystem hitParticles;
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float attackDelay = 3f;
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float enemyDamage = 10f;
    [SerializeField] private float enemyHitStrength = 5f;
    [SerializeField] private float enemyHitTime = 0.2f;

    private bool isWaiting = false;
    private bool isAttacking = false;
    private float timer = 0f;

    void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null) player = playerObj.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange && !isAttacking && !isWaiting)
        {
            isWaiting = true;
            timer = 0f;
        }

        if (isWaiting)
        {
            timer += Time.deltaTime;
            if (timer >= attackDelay)
            {
                isWaiting = false;
                isAttacking = true;
            }
        }

        if (isAttacking)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController pc = collision.GetComponent<PlayerController>();
        if (pc != null)
        {
            // Reproduce las partículas en el punto de contacto
            if (hitParticles != null)
            {
                Vector2 contactoPunto = collision.ClosestPoint(transform.position);
                ParticleSystem particles = Instantiate(hitParticles, contactoPunto, Quaternion.identity);
                particles.Play();
                Destroy(particles.gameObject, particles.main.duration + particles.main.startLifetime.constantMax);
            }

            // Aplica los valores de daño y retroceso directamente al PlayerController
            pc.hitTime = enemyHitTime;
            pc.hitForce = enemyHitStrength;
            pc.TakeDamage(enemyDamage);

            // Define la dirección del empuje según la posición de Sara frente al enemigo
            if (collision.transform.position.x < transform.position.x)
            {
                pc.hitFromRight = true;
            }
            else
            {
                pc.hitFromRight = false;
            }

            // Destruye al enemigo al impactar a Sara
            Destroy(gameObject);
        }
    }
}