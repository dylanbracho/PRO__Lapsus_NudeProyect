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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyRb = GetComponent<Rigidbody2D>();
        actualObjective = enemyMovementPoints[0];
        isFacingRight = false;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToObjective = Vector2.Distance(transform.position, actualObjective.position);
        if (distanceToObjective < detectionRadius)
        {
            if (actualObjective == enemyMovementPoints[0])
            {
                actualObjective = enemyMovementPoints[1];
            }
            else if (actualObjective == enemyMovementPoints[1])
            {
                actualObjective = enemyMovementPoints[0];
            }
        }
        Vector2 direction = (actualObjective.position - transform.position).normalized;
        int roundDirection = Mathf.RoundToInt(direction.x);
        movement = new Vector2(roundDirection, 0);

        if (roundDirection < 0 && isFacingRight)        // Va a la izquierda pero mira a la derecha.
        {
            Flip();
        }
        else if (roundDirection > 0 && !isFacingRight)  // Va a la derecha pero mira a la izquierda.
        {
            Flip();
        }

        enemyRb.MovePosition(enemyRb.position + movement * enemySpeed * Time.deltaTime);
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
        // Preguntamos: "¿con lo que choqué tiene un componente PlayerController?"
        // Si GetComponent no encuentra el componente, devuelve null (nada).
        // Entonces "!= null" significa "SÍ lo encontró" -> choqué con el jugador.
        //
        // Esta es una alternativa a usar CompareTag("Player"). Es más segura,
        // porque garantiza que el objeto realmente tiene el script que vamos a
        // usar y no solo una etiqueta que alguien pudo asignar mal.
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            // Guardamos la referencia al script del jugador en una variable,
            // para no tener que escribir GetComponent una y otra vez.
            // Ahora, a través de "player", podemos usar sus métodos y variables.
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            // Llamamos al método TakeDamage() que está EN EL OTRO SCRIPT.
            // Le enviamos enemyDamage como parámetro: el enemigo decide cuánto
            // daño hace, y el jugador se encarga de restarlo y actualizar la UI.
            player.TakeDamage(enemyDamage);

            // Le ESCRIBIMOS variables al jugador para configurar su empujón.
            // Recuerden que en el PlayerController, mientras hitTime sea mayor
            // que 0, el jugador pierde el control y sale volando.
            player.hitTime = 0.5f;              // Medio segundo de aturdimiento.
            player.hitForce = enemyStrength;    // Qué tan fuerte sale volando.

            // ---- ¿HACIA DÓNDE DEBE SALIR VOLANDO EL JUGADOR? ----
            // Comparamos las posiciones en X para saber quién está a la izquierda.
            // collision.transform.position.x -> X del JUGADOR
            // transform.position.x           -> X del ENEMIGO (o sea, la mía)

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

            public void TakeDamage(float damage)
            {
                 health -= damage;   // -= significa "réstate a ti mismo".
                            
            }
        }
    }


}
