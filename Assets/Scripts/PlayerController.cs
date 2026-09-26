using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float direction;
    public float speed;
    public Rigidbody2D rb;
    public float jumpForce;
    public bool canJump;
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    public bool isFacingRight;
    public float health = 0;
    public float hitForce;      // Con cuánta fuerza sale volando el jugador al ser golpeado.
    public float hitTime;       // Cuántos segundos dura el empujón. Mientras sea > 0, el
                                // jugador NO puede controlarse (está "aturdido").
    public bool hitFromRight;
    public float dashSpeed = 20f;
    public float dashTime = 0.2f;
    public float dashCooldown = 1f;
    [HideInInspector] public bool isInvincible = false;
    private bool isDashing = false;
    private bool canDash = true;
    public Collider2D hurtBox1;
    public Collider2D hurtBox2;
    public LayerMask enemyLayer;
    private float lastMoveX = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        canJump = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        //canDash = !canJump;
      
        

        if (!isFacingRight && direction > 0f)
        {
            Flip();
        }
        else if (isFacingRight && direction < 0f)
        {
            Flip();
        }

        if (isDashing)
        {
            return;
        }

        if (hitTime <= 0)
        {
            // MOVIMIENTO NORMAL
            // linearVelocity es la velocidad del Rigidbody2D: un Vector2 (X, Y).
            // Le asignamos un Vector2 nuevo donde:
            //   X = direction * speed  -> la dirección que presiona el jugador por su velocidad.
            //   Y = rb.linearVelocityY -> ¡OJO! Conservamos la velocidad vertical que ya tenía.
            // Si en Y pusiéramos 0, borraríamos el salto y la gravedad cada frame.
            rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocityY);
        }
        else // Me golpearon: el jugador pierde el control por unos instantes.
        {
            if (hitFromRight) // Me pegaron por la derecha, entonces salgo hacia la IZQUIERDA.
            {
                // hitForce negativo en X = empujón hacia la izquierda.
                // hitForce positivo en Y = también lo levanta un poco (efecto de "salto" del golpe).
                rb.linearVelocity = new Vector2(-hitForce, hitForce);
            }
            else if (!hitFromRight) // Me pegaron por la izquierda...
            {
                // Me pegaron por la izquierda, entonces salgo hacia la DERECHA.
                rb.linearVelocity = new Vector2(hitForce, hitForce);
            }

            // Descontamos el tiempo del cronómetro del golpe.
            // Time.deltaTime = segundos que pasaron desde el frame anterior.
            // Restarlo cada frame hace que hitTime baje en SEGUNDOS REALES,
            // sin importar si el juego corre a 30, 60 o 144 FPS.
            hitTime -= Time.deltaTime;
        }

    }

    
    public void Dash(InputAction.CallbackContext context)
    {
            //Debug.Log("verga");
        if (context.performed && canDash  && !canJump)
        {
            Debug.Log("Simon lo mejor");
            StartCoroutine(DoDash());
            StartCoroutine(DashCooldown());
        }
    }

    private IEnumerator DashCooldown()
    {
        Debug.Log("hdcskhk");
        yield return new WaitForSeconds(dashCooldown);
        Debug.Log("Me da un fomo");
        canDash = true;

    }


    private IEnumerator DoDash()
    {
        isDashing = true;
        canDash = false;
        isInvincible = true;
        int enemyLayerIndex = LayerMaskToLayer(enemyLayer);
        Physics2D.IgnoreLayerCollision(gameObject.layer, enemyLayerIndex, true);
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f; // Desactiva la gredad durante el dash
        rb.linearVelocity += new Vector2(lastMoveX * dashSpeed, 0f); // Dash en la dirección del último movimiento
        yield return new WaitForSeconds(dashTime);
        rb.gravityScale = originalGravity; // Restaura la gravedad
        isDashing = false;
        isInvincible = false;
        Physics2D.IgnoreLayerCollision(gameObject.layer, enemyLayerIndex, false);
    }
    public void Mover(InputAction.CallbackContext context)
    {

        float x = context.ReadValue<Vector2>().x;
        direction = x;
        if (x != 0f) lastMoveX = x;

    }

    public void Salto(InputAction.CallbackContext context)
    {
        if (context.performed && canJump)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
           // canDash = true;
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;             // Cambia el estado de la variable
                                                    // (si era true pasa a false y al revés).

        Vector3 localScale = transform.localScale;  // Guarda la escala actual
                                                    // (transform es el componente de posición,
                                                    //  rotación y escala del GameObject).
        localScale.x *= -1f;                        // Invierte el eje X (espejo)
                                                    // *= significa "multiplícate por".
        transform.localScale = localScale;          // Aplica la nueva escala
                                                    // Hay que reasignarlo completo porque
                                                    // no se puede modificar localScale.x directo.
    }
    public void TakeDamage(float damage)
    {
        if (isInvincible == true)
        {
            return;
        }
        health -= damage;  

    }

    private int LayerMaskToLayer(LayerMask mask)
    {
        int bitmask = mask.value;
        int layerNumber = 0;
        while (bitmask > 1)
        {
            bitmask >>= 1;
            layerNumber++;
        }
        return layerNumber;
    }


}

