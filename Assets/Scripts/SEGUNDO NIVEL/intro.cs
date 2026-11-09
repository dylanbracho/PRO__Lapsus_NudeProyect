using UnityEngine;

public class PromQueenIntro : MonoBehaviour
{
    [SerializeField] private DialogueTrigger dialogueTrigger; // Arrastra el objeto que tiene el DialogueTrigger con las líneas de la Prom Queen
    [SerializeField] private PromQueenBoss bossScript;        // El script de combate de la Prom Queen
    [SerializeField] private MonoBehaviour playerMovementScript; // El script de movimiento de Sara (para congelarla)

    private DialogueManager dialogueManager;
    private bool triggered = false;
    private bool dialogueStarted = false;

    void Start()
    {
        dialogueManager = FindFirstObjectByType<DialogueManager>();

        // Asegúrate de que el boss esté desactivado al inicio para que no ataque mientras caminas
        if (bossScript != null)
            bossScript.enabled = false;
    }

    void Update()
    {
        // Si el diálogo ya arrancó, vigilamos cuándo se cierra la caja de texto (cuando el animador de la UI cierra el diálogo)
        if (dialogueStarted && dialogueManager != null)
        {
            // Verificamos si el animator del DialogueManager cerró la caja ("IsOpen" pasa a falso)
            if (dialogueManager.animator != null && !dialogueManager.animator.GetBool("IsOpen"))
            {
                dialogueStarted = false;
                StartBossFight();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!triggered && (collision.CompareTag("Player") || collision.GetComponent<PlayerController>() != null))
        {
            triggered = true;
            TriggerIntroDialogue();
        }
    }

    void TriggerIntroDialogue()
    {
        Debug.Log("Sara llega con la Prom Queen. Iniciando diálogo...");

        // 1. Congela a Sara para que no se mueva
        if (playerMovementScript != null)
            playerMovementScript.enabled = false;

        // 2. Dispara el diálogo usando tu propio DialogueTrigger
        if (dialogueTrigger != null)
        {
            dialogueTrigger.TriggerDialogue();
            dialogueStarted = true;
        }
        else
        {
            // Si no hay trigger asignado, arranca la pelea directo de emergencia
            StartBossFight();
        }
    }

    void StartBossFight()
    {
        // 1. Devuelve el movimiento a Sara
        if (playerMovementScript != null)
            playerMovementScript.enabled = true;

        // 2. Activa el script de la Prom Queen para que empiece el combate y las fases
        if (bossScript != null)
            bossScript.enabled = true;

        Debug.Log("¡Diálogo finalizado! Comienza la masacre de la Prom Queen.");

        // Destruye este trigger para que no se vuelva a activar
        Destroy(gameObject);
    }
}