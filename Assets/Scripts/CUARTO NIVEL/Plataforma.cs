using UnityEngine;

public class Plataforma : MonoBehaviour
{
    [SerializeField] private float metaP;    // Altura máxima a la que sube
    [SerializeField] private float speedP;   // Velocidad de movimiento

    [Header("Configuración de Respawn de Sara")]
    [SerializeField] private bool actualizarSpawnAlLlegar = true;

    [Header("Coordenadas del Nuevo Spawn")]
    [Tooltip("Coloca aquí la posición X y Y exacta donde quieres que aparezca Sara al llegar arriba.")]
    [SerializeField] private Vector2 nuevaPosicionSpawn;

    private float startY;
    private bool isMoving = false;
    private bool yaSubio = false;

    void Start()
    {
        startY = transform.position.y;
    }

    void Update()
    {
        if (isMoving)
        {
            float currentY = Mathf.MoveTowards(transform.position.y, metaP, speedP * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, currentY, transform.position.z);

            if (Mathf.Approximately(transform.position.y, metaP))
            {
                isMoving = false;
                Debug.Log("¡La plataforma llegó a su tope!");

                // 1. Actualizamos el Spawn Point de Sara
                if (actualizarSpawnAlLlegar)
                {
                    GameObject objetoSpawn = GameObject.Find("spawnpoint");

                    if (objetoSpawn != null)
                    {
                        objetoSpawn.transform.position = new Vector3(nuevaPosicionSpawn.x, nuevaPosicionSpawn.y, objetoSpawn.transform.position.z);
                        Debug.Log($"¡Spawnpoint actualizado a X: {nuevaPosicionSpawn.x}, Y: {nuevaPosicionSpawn.y}!");
                    }
                }

                // 2. BUSCAMOS Y ACTIVAMOS AL "Heart Enemy" AUTOMÁTICAMENTE
                GameObject jefeObj = GameObject.Find("Heart Enemy");

                if (jefeObj != null)
                {
                    BasicMov mov = jefeObj.GetComponent<BasicMov>();
                    if (mov != null) mov.ActivarJefe();

                    AtaquesBoss atq = jefeObj.GetComponent<AtaquesBoss>();
                    if (atq != null) atq.ActivarAtaques();

                    Debug.Log("¡Heart Enemy y sus ataques activados con éxito!");
                }
                else
                {
                    Debug.LogWarning("No se encontró ningún objeto llamado 'Heart Enemy' en la escena.");
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isMoving && !yaSubio)
        {
            isMoving = true;
            yaSubio = true;
            Debug.Log("¡Activando: Subiendo plataforma por única vez!");
        }
    }
}