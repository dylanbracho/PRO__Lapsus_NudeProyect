using System.Collections;
using UnityEngine;

public class GordoController : MonoBehaviour
{
    Vector2 movement; 
    public float enemySpeed;           
    public Rigidbody2D enemyRb;
    public Transform player;
    public float keepDistance = 4f;    // Distance it tries to maintain
    public float distanceBuffer = 0.5f; // Dead zone so it doesn't jitter back and forth
    public float chaseRange = 10f;
    public bool isFacingRight;
    [SerializeField] private float enemyDamage;     
    [SerializeField] private float enemyStrength;
    bool isAttacking = false;
    //[SerializeField] private Animator animator;
    [SerializeField] private GameObject whipHitbox;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private JumpEnemy salto;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyRb = GetComponent<Rigidbody2D>();
        isFacingRight = false;

        if (player == null)
        {
            player = FindFirstObjectByType<PlayerController>().transform;
           
        }
    }

    void Update()
    {
        if (player == null) return;

        if (!isAttacking)
        {
            StartCoroutine(Attack());

        }
        

        float xDiff = player.position.x - transform.position.x;
        float distance = Mathf.Abs(xDiff);
        float towardPlayer = Mathf.Sign(xDiff);   // -1 = player is left, +1 = right

        // Always face the player, even while backing away
        if ((towardPlayer < 0 && isFacingRight) || (towardPlayer > 0 && !isFacingRight))
        {
            Flip();
        }

        float dir = 0f;
        if (distance <= chaseRange)
        {
            if (distance > keepDistance + distanceBuffer)
                dir = towardPlayer;     // Too far: approach
            else if (distance < keepDistance - distanceBuffer)
                dir = -towardPlayer;    // Too close: back away
        }

        enemyRb.MovePosition(enemyRb.position + new Vector2(dir, 0) * enemySpeed * Time.deltaTime);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;             // Cambia el estado de la variable

        Vector3 localScale = transform.localScale;  // Guarda la escala actual
        localScale.x *= -1f;                        // Invierte el eje X (espejo)
        transform.localScale = localScale;          // Aplica la nueva escala
    }

    void OnTriggerEnter2D(Collider2D collision)
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

        if (whipHitbox != null && isAttacking == false)
        {
            if (salto.busy == true) yield break;
            whipHitbox.SetActive(true);
            OnTriggerEnter2D(whipHitbox.GetComponent<Collider2D>());
            isAttacking = true;
        }
        yield return new WaitForSeconds(0.5f);
        whipHitbox.SetActive(false);
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }

}
