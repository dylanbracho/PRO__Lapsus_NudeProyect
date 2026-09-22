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

