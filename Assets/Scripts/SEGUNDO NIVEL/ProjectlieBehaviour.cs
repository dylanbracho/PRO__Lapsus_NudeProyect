using UnityEngine;

public class ProjectlieBehaviour : MonoBehaviour
{
    

    public float speed = 5f;
    [SerializeField] private float enemyDamage = 10f;
    [SerializeField] private float enemyStrength = 5f;
    private Vector2 direction = Vector2.right;
    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (direction.x < 0 ? -1 : 1);
        transform.localScale = scale;

        // Tilt the sprite toward the direction it's flying
        float angle = Mathf.Atan2(direction.y, Mathf.Abs(direction.x)) * Mathf.Rad2Deg;
        if (direction.x < 0) angle = -angle;   // the mirrored scale flips the rotation
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    private void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }
 

    private void OnTriggerEnter2D(Collider2D other)
    {
        
            Destroy(gameObject);
            if (other.gameObject.GetComponent<PlayerController>() != null)
            {
                // Guardamos la referencia al script del jugador en una variable,
                // para no tener que escribir GetComponent una y otra vez.
                // Ahora, a través de "player", podemos usar sus métodos y variables.
                PlayerController player = other.gameObject.GetComponent<PlayerController>();

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

                if (other.transform.position.x <= transform.position.x)
                {
                    // El jugador está a MI izquierda, así que el golpe le llegó
                    // desde su derecha -> debe salir volando hacia la izquierda.
                    player.hitFromRight = true;
                }
                else if (other.transform.position.x > transform.position.x)
                {
                    // El jugador está a MI derecha: el golpe le llegó por la
                    // izquierda -> debe salir volando hacia la derecha.
                    player.hitFromRight = false;
                }


            }
        
    }
}
