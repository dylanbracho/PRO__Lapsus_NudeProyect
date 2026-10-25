using UnityEngine;

public class Plataforma : MonoBehaviour
{
    [SerializeField] private float metaP;    // Altura máxima a la que sube
    [SerializeField] private float speedP;   // Velocidad de movimiento

    private float startY;          // Posición inicial (abajo)
    private float targetY;         // Hacia dónde se dirige actualmentes
    private bool isMoving = false; // Para saber si ya está en movimiento
    private bool debeSubir = true; // Interruptor: true = toca subir, false = toca bajar

    void Start()
    {
        // Guardamos la posición Y en la que colocaste la plataforma al iniciar el juego
        startY = transform.position.y;
        targetY = startY;
    }

    void Update()
    {
        // Si la plataforma tiene luz verde para moverse
        if (isMoving)
        {
            // Movemos la plataforma suavemente hacia el objetivo actual (Y)
            float currentY = Mathf.MoveTowards(transform.position.y, targetY, speedP * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, currentY, transform.position.z);

            // Si ya llegó a su destino exacto, detenemos el movimiento
            if (Mathf.Approximately(transform.position.y, targetY))
            {
                isMoving = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verificamos que sea Sara la que entra y que la plataforma no esté ya en movimiento
        if (collision.CompareTag("Player") && !isMoving)
        {
            // Dependiendo del interruptor, decidimos el destino
            if (debeSubir)
            {
                targetY = metaP;  // Su objetivo es subir
                Debug.Log("¡Activando: Subiendo plataforma!");
            }
            else
            {
                targetY = startY; // Su objetivo es bajar al punto inicial
                Debug.Log("¡Activando: Bajando plataforma!");
            }

            // Arrancamos el movimiento
            isMoving = true;

            // Invertimos el interruptor para que la *siguiente* vez haga lo opuesto
            debeSubir = !debeSubir;
        }
    }
}