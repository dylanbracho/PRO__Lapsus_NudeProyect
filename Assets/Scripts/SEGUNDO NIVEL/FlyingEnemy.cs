using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private ParticleSystem hitParticles;
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float attackDelay = 3f;
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float enemyDamage = 10f;
    [SerializeField] private float enemyHitStrengthX = 5f;
    [SerializeField] private float enemyHitStrengthY = 5f;
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
        if (collision.transform == player)
        {
            if (hitParticles != null)
            {
                Vector2 contactoPunto = collision.ClosestPoint(transform.position);

                ParticleSystem particles = Instantiate(hitParticles, contactoPunto, Quaternion.identity);
                particles.Play();
                Destroy(particles.gameObject, particles.main.duration + particles.main.startLifetime.constantMax);
            }

            Vector2 hitDirection = (collision.transform.position - transform.position).normalized;

            collision.transform.SendMessage(
                "RecibirDanioConKnockback",
                new HitData(enemyDamage, enemyHitStrengthX, enemyHitStrengthY, enemyHitTime, hitDirection),
                SendMessageOptions.DontRequireReceiver
            );

            Destroy(gameObject);
        }
    }
}

public struct HitData
{
    public float damage;
    public float strengthX;
    public float strengthY;
    public float hitTime;
    public Vector2 direction;

    public HitData(float dmg, float sx, float sy, float time, Vector2 dir)
    {
        damage = dmg;
        strengthX = sx;
        strengthY = sy;
        hitTime = time;
        direction = dir;
    }
}