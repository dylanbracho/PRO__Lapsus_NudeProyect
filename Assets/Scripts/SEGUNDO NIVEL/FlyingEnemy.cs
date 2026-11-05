using UnityEngine;
using System.Collections;

public class FlyingEnemy : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private ParticleSystem hitParticles;
    [SerializeField] private Collider2D triggerCollider;
    [SerializeField] private Collider2D detectionCollider; // El collider de zona que puso tu profe
    [SerializeField] private float attackDelay = 3f;
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float enemyDamage = 10f;
    [SerializeField] private float enemyHitStrength = 5f;
    [SerializeField] private float enemyHitTime = 0.2f;

    [SerializeField] private SpriteRenderer spriteRenderer;

    private bool isWaiting = false;
    private bool isAttacking = false;
    private float timer = 0f;
    private bool isPlayerInRange = false;
    private Coroutine flashCoroutine;

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

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    void Update()
    {
        if (player == null) return;

        if (isPlayerInRange && !isAttacking && !isWaiting)
        {
            isWaiting = true;
            timer = 0f;

            if (spriteRenderer != null)
            {
                if (flashCoroutine != null) StopCoroutine(flashCoroutine);
                flashCoroutine = StartCoroutine(FlashRedRoutine(attackDelay));
            }
        }

        if (isWaiting)
        {
            timer += Time.deltaTime;
            if (!isPlayerInRange)
            {
                timer = 0f;
                isWaiting = false;
                if (flashCoroutine != null) StopCoroutine(flashCoroutine);
                if (spriteRenderer != null) spriteRenderer.color = Color.white;
                return;
            }
            if (timer >= attackDelay)
            {
                isWaiting = false;
                isAttacking = true;
                if (spriteRenderer != null) spriteRenderer.color = Color.white;
            }
        }

        if (isAttacking)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
    }

    IEnumerator FlashRedRoutine(float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float remainingTime = duration - elapsed;
            float flashSpeed = Mathf.Lerp(0.08f, 0.25f, remainingTime / duration);

            if (spriteRenderer != null) spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(flashSpeed);
            if (spriteRenderer != null) spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(flashSpeed);

            elapsed += flashSpeed * 2f;
        }
        if (spriteRenderer != null) spriteRenderer.color = Color.red;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si el objeto entra específicamente en el collider de detección del profe
        if (detectionCollider != null && collision == detectionCollider) return;

        if (collision.gameObject.CompareTag("Player") || collision.GetComponentInParent<PlayerController>() != null)
        {
            // Validamos si entró al area de detección del profe o al trigger general
            if (detectionCollider == null || collision.IsTouching(detectionCollider))
            {
                isPlayerInRange = true;
            }
        }

        PlayerController pc = collision.GetComponentInParent<PlayerController>();
        if (pc != null && isAttacking)
        {
            TriggerHit(pc, collision.ClosestPoint(transform.position));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.GetComponentInParent<PlayerController>() != null)
        {
            // Si sale del área de detección
            if (detectionCollider == null || !collision.IsTouching(detectionCollider))
            {
                isPlayerInRange = false;
                if (flashCoroutine != null) StopCoroutine(flashCoroutine);
                if (spriteRenderer != null) spriteRenderer.color = Color.white;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController pc = collision.gameObject.GetComponent<PlayerController>();
        if (pc == null) pc = collision.gameObject.GetComponentInParent<PlayerController>();

        if (pc != null)
        {
            Vector2 contactoPunto = collision.collider.ClosestPoint(transform.position);
            TriggerHit(pc, contactoPunto);
        }
    }

    private void TriggerHit(PlayerController pc, Vector2 hitPoint)
    {
        if (hitParticles != null)
        {
            ParticleSystem particles = Instantiate(hitParticles, hitPoint, Quaternion.identity);
            particles.Play();
            Destroy(particles.gameObject, particles.main.duration + particles.main.startLifetime.constantMax);
        }

        pc.hitTime = enemyHitTime;
        pc.hitForce = enemyHitStrength;
        pc.TakeDamage(enemyDamage);

        if (player.position.x < transform.position.x)
        {
            pc.hitFromRight = true;
        }
        else
        {
            pc.hitFromRight = false;
        }

        Debug.Log("¡Golpea al jugador, muestra partículas y se destruye!");
        Destroy(gameObject);
    }
}