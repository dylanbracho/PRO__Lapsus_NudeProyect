using UnityEngine;

public class SimpleDialogueTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.GetComponent<PlayerController>() != null)
        {
            DialogueTrigger trigger = GetComponent<DialogueTrigger>();
            if (trigger != null)
            {
                trigger.TriggerDialogue();
                // Destruye este trigger para que no se repita el diálogo al caminar cerca
                Destroy(gameObject);
            }
        }
    }
}