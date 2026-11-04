using System.Collections;
using UnityEngine;

public class CaogulitoController : MonoBehaviour
{
    Vector2 movement;
    public float enemySpeed;
    public Rigidbody2D enemyRb;
    public float detectionRadius = 0.5f;
    public Transform actualObjective;
    public Transform[] enemyMovementPoints;
    public bool isFacingRight;
    [SerializeField] private float enemyDamage;
    [SerializeField] private float enemyStrength;
    private Animator animator;
    [SerializeField] private Collider2D hitbox;
    [SerializeField] private Transform player;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackCooldown = 2f;
    private bool isAttacking = false;
    private bool canAttack = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyRb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        actualObjective = enemyMovementPoints[0];
        if (player == null)
            player = FindFirstObjectByType<PlayerController>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAttacking) return;   // stand still while attacking

        // Attack if the player is close
        if (canAttack && Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            StartCoroutine(Attack());
            return;
        }

        // ---- your existing patrol code goes here, unchanged ----
        if (Mathf.Abs(actualObjective.position.x - transform.position.x) < detectionRadius)
        {
            actualObjective = (actualObjective == enemyMovementPoints[0])
                ? enemyMovementPoints[1]
                : enemyMovementPoints[0];
        }

        float dir = Mathf.Sign(actualObjective.position.x - transform.position.x);

        if ((dir < 0 && isFacingRight) || (dir > 0 && !isFacingRight))
            Flip();

        enemyRb.MovePosition(enemyRb.position + new Vector2(dir, 0) * enemySpeed * Time.deltaTime);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;             // Cambia el estado de la variable

        Vector3 localScale = transform.localScale;  // Guarda la escala actual
        localScale.x *= -1f;                        // Invierte el eje X (espejo)
        transform.localScale = localScale;          // Aplica la nueva escala
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
      
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            
            player.TakeDamage(enemyDamage);

            
            player.hitTime = 0.5f;              // Medio segundo de aturdimiento.
            player.hitForce = enemyStrength;    // Qué tan fuerte sale volando.

            if (collision.transform.position.x <= transform.position.x)
            {
                // El jugador está a MI izquierda, así que el golpe le llegó
                // desde su derecha -> debe salir volando hacia la izquierda.
                player.hitFromRight = true;
            }
            else if (collision.transform.position.x > transform.position.x)
            {
                // El jugador está a MI derecha: el golpe le llegó por la
                // izquierda -> debe salir volando hacia la derecha.
                player.hitFromRight = false;
            }

            
        }
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        canAttack = false;

        // Face the player before swinging
        float dirToPlayer = Mathf.Sign(player.position.x - transform.position.x);
        if ((dirToPlayer < 0 && isFacingRight) || (dirToPlayer > 0 && !isFacingRight))
            Flip();

        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.5f);   // wind-up
        hitbox.enabled = true;
        yield return new WaitForSeconds(0.5f);   // active frames
        hitbox.enabled = false;

        isAttacking = false;                     // can walk again
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;                        // can attack again
    }


}
