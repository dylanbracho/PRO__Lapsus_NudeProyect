using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class EspejoController : MonoBehaviour
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
    public ProjectlieBehaviour proyectilPrefab;
    private Animator animator;
    public Transform launchOffset;
    [SerializeField] private Transform player;
    public float timeBetweenShots = 0.8f;
    public float attackRadius = 5f;    // How close the player must be for the enemy to shoot
    public LayerMask playerLayer;
    [SerializeField] private float attackRange = 1.5f;
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
        StartCoroutine(LanzarProyectilCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        if (isAttacking) return;   // stand still while attacking


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
                
                player.hitFromRight = true;
            }
            else if (collision.transform.position.x > transform.position.x)
            {
                
                player.hitFromRight = false;
            }

            
        }
    }

    private IEnumerator LanzarProyectilCoroutine()
    {
        while (true)
        {
            Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, attackRadius, playerLayer);

            if (playerCollider != null && IsInFront(playerCollider.transform))
            {
                isAttacking = true;
                canAttack = false;
                animator.SetTrigger("Attack");
                LanzarProyectil(playerCollider.transform);
                yield return new WaitForSeconds(timeBetweenShots);
                canAttack = true;
            }
            else
            {
                yield return null;
            }
        }
    }

    private void LanzarProyectil(Transform target)
    {
        ProjectlieBehaviour proyectil = Instantiate(proyectilPrefab, launchOffset.position, Quaternion.identity);

        Vector2 direction = (target.position - launchOffset.position).normalized;
        proyectil.SetDirection(direction);
        isAttacking = false;
    }

    private bool IsInFront(Transform target)
    {
        float xDifference = target.position.x - transform.position.x;
        return isFacingRight ? xDifference >= 0 : xDifference <= 0;
    }

}
