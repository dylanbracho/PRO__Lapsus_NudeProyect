using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private ParticleSystem hitParticles;
    [SerializeField] private Collider2D triggerCollider;
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float attackDelay = 3f;
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float enemyDamage = 10f;
    [SerializeField] private float enemyHitStrength = 5f;
    [SerializeField] private float enemyHitTime = 0.2f;

    [SerializeField] private Collider2D detectionCollider; // Arrastra aquí el collider que hace de zona
    private bool isWaiting = false;
    private bool isAttacking = false;
    private float timer = 0f;

    //PRUEBEN PORFA
    bool isPlayerInRange = false;

    void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null) player = playerObj.transform;
        }

        if (triggerCollider == null)
        {
            triggerCollider = GetComponent<Collider2D>();
        }
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (isPlayerInRange && !isAttacking && !isWaiting)
        {
            isWaiting = true;
            timer = 0f;
        }


        if (isWaiting)
        {
            timer += Time.deltaTime;
            if (!isPlayerInRange)
            {
                timer = 0f;
                return;
            }
            if (timer >= attackDelay)
            {
                Debug.Log("Attack initiated");
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
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }

        //PlayerController pc = collision.GetComponentInParent<PlayerController>();
        //if (pc != null)
        //{
        //    if (hitParticles != null)
        //    {
        //        Vector2 contactoPunto = collision.ClosestPoint(transform.position);
        //        ParticleSystem particles = Instantiate(hitParticles, contactoPunto, Quaternion.identity);
        //        particles.Play();
        //        Destroy(particles.gameObject, particles.main.duration + particles.main.startLifetime.constantMax);
        //    }

        //    pc.hitTime = enemyHitTime;
        //    pc.hitForce = enemyHitStrength;
        //    pc.TakeDamage(enemyDamage);


        //    if (collision.transform.position.x < transform.position.x)
        //    {
        //        pc.hitFromRight = true;
        //    }
        //    else
        //    {
        //        pc.hitFromRight = false;
        //    }

        //    //Destroy(gameObject);
        //}
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController pc = collision.gameObject.GetComponent<PlayerController>();
            pc.TakeDamage(enemyDamage);
            Debug.Log("GOLPEA Y DESAPARECE");
            Destroy(gameObject);
        }
    }
}