using UnityEngine;
using UnityEngine.InputSystem;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        canJump = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        rb.linearVelocity = new Vector2(speed * direction, rb.linearVelocityY);

        if (!isFacingRight && direction > 0f)
        {
            Flip();
        }
        else if (isFacingRight && direction < 0f)
        {
            Flip();
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
    public void Mover(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>().x;
    }

    public void Salto(InputAction.CallbackContext context)
    {
        if (context.performed && canJump)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
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

}

