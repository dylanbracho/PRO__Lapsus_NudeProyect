using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

using UnityEngine.UI;
using TMPro;

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
    
    public float hitForce;      // Con cuánta fuerza sale volando el jugador al ser golpeado.
    public float hitTime;       // Cuántos segundos dura el empujón. Mientras sea > 0, el
                                // jugador NO puede controlarse (está "aturdido").
    public bool hitFromRight;
    public float dashSpeed = 20f;
    public float dashTime = 0.2f;
    public float dashCooldown = 1f;
    [HideInInspector] public bool isInvincible = false;
    private bool isDashing = false;
    public bool canDash = true;
    public Collider2D hurtBox1;
    public Collider2D hurtBox2;
    public LayerMask enemyLayer;
    private float lastMoveX = 1f;
    private bool isAttacking = false;

    //ataque

    public InputActionReference attackAction;
    public Animator animator;
    public GameObject whipHitbox;

    public float attackCooldown = 0.5f;
    public float hitboxActiveTime = 0.2f;

    private bool canAttack = true;

    //vida
    public float health;
    public float maxhealth;  
    public bool muerta = false;
    

    //ui, spawn, cosas de ale

    public UIManager uimanager;
  
    public GameObject spawnpoint;

    public Slider healthBarSlider;
    public TextMeshProUGUI vidaentexto;

    // NPC
    public DialogueTrigger activeNPC;
    public Animator holasamy;

    //Proyectil
    public ProjectlieBehaviour proyectilPrefab;
    public Transform launchOffset;

    //Mazo
    public InputActionReference attackActionMazo;
    [SerializeField] private GameObject mazoHitbox;
    private float mazoCooldown = 1f;
    private float mazoHitboxActiveTime = 0.2f;

    //Stun
    public bool isStunned;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxhealth;
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

        if (isStunned)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);  
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


        //spawn cuando muerte 

        if (health <= 0 && !muerta) // Usamos !muerta para que solo se ejecute una vez al morir
        {
            muerta = true;
            StartCoroutine(RespawnRoutine());
            
        }

    }

    private IEnumerator RespawnRoutine()
    {
       
        rb.linearVelocity = Vector2.zero;
        enabled = optionalDisableControlsOnDeath(); 
        yield return null;

        // Reposicionar al jugador
        transform.position = spawnpoint.transform.position;

        // Restaurar salud y estados
        health = maxhealth;
        hitTime = 0f;          // Quita el estado de aturdimiento del golpe
        isDashing = false;     // Por si estaba dasheando
        isInvincible = false;
        healthBarSlider.value = health;
        healthBarSlider.maxValue = maxhealth;
        vidaentexto.text = $"HP: {health}/{maxhealth}";

        muerta = false;
    }

    private bool optionalDisableControlsOnDeath()
    {
        return true;
    }


    public void Dash(InputAction.CallbackContext context)
    {
            //Debug.Log("verga");
        if (context.performed && canDash  && !canJump)
        {
            Debug.Log("Simon lo mejor");
            animator.SetTrigger("Dash");
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
        animator.SetFloat("Direccion", direction);
        

    }

    public void Salto(InputAction.CallbackContext context)
    {
        if (context.performed && canJump)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
           // canDash = true;
        }
    }

    public void ActivarDialogo(InputAction.CallbackContext context)
    {
        if (context.performed && activeNPC != null)
        {
            Debug.Log("Hola Dylan");
            activeNPC.TriggerDialogue();
            
        }
    }

    public void LanzarProyectil(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ProjectlieBehaviour proyectil = Instantiate(proyectilPrefab, launchOffset.position, Quaternion.identity);
            proyectil.SetDirection(isFacingRight ? Vector2.right : Vector2.left);
        }
    }

    public void Activarmenu(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            uimanager.Activarmenu();
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;             

        Vector3 localScale = transform.localScale;  
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
        healthBarSlider.value = health;
        healthBarSlider.maxValue = maxhealth;
        vidaentexto.text = $"HP: {health}/{maxhealth}";

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

    private void OnEnable()
    {
        if(attackAction != null)
        {
            attackAction.action.Enable();
            attackAction.action.performed += OnAttack;
        }
        if(attackActionMazo != null)
        {
            attackActionMazo.action.Enable();
            attackActionMazo.action.performed += AttackMazo;

        }

    }

    private void OnDisable()
    {
        attackAction.action.performed -= OnAttack;
        attackAction.action.Disable();
        attackActionMazo.action.performed -= AttackMazo;
        attackActionMazo.action.Disable();
        isAttacking = false;
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        if (!canAttack)
            return;
        isAttacking = true;
        canAttack = false;
        animator.SetTrigger("Attack");

        whipHitbox.SetActive(true);
        Invoke(nameof(DisableWhip), hitboxActiveTime);
        Invoke(nameof(ResetAttack), attackCooldown);
    }

    void AttackMazo(InputAction.CallbackContext context)
    {
        if (!canAttack)
            return;
        if (!canJump)
        {
            isAttacking = true;
            canAttack = false;
            animator.SetTrigger("AttackMazo");
            mazoHitbox.SetActive(true);
            Debug.Log("AttackMazo triggered");
            Invoke(nameof(DisableMazo), mazoHitboxActiveTime);
            Invoke(nameof(ResetAttack), mazoCooldown);
        }
    }

    private void DisableWhip()
    {
        whipHitbox.SetActive(false);
    }

    private void DisableMazo()
    {
        mazoHitbox.SetActive(false);
    }

    private void ResetAttack()
    {
        canAttack = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("NPC"))
        {
            activeNPC = collision.gameObject.GetComponent<DialogueTrigger>();
            holasamy.SetTrigger("algop");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("NPC"))
        {
            activeNPC = null;
            holasamy.SetTrigger("algop");
        }
    }

    public void Stun(float duration)
    {
        if (isStunned) return;   // Ignore hits while already stunned
        StartCoroutine(StunRoutine(duration));
    }

    private IEnumerator StunRoutine(float duration)
    {
        isStunned = true;
        bool couldJump = canJump;
        bool couldDash = canDash;
        canJump = false;
        canDash = false;

        rb.linearVelocity = Vector2.zero;   // use rb.velocity on older Unity versions

        yield return new WaitForSeconds(duration);

        canJump = couldJump;
        canDash = couldDash;
        isStunned = false;
    }


}


