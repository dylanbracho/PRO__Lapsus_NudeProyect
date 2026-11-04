using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement; // Importante para cambiar de escena

public class HeartLocketDoor : MonoBehaviour
{
    [SerializeField] private GameObject keyPromptObject; // El aviso de la "E"
    [SerializeField] private Collider2D detectionArea;    // El área de colisión
    [SerializeField] private string targetSceneName;     // Escribe aquí el nombre exacto de tu escena de la Prom Queen
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip lockedSound;

    private bool isPlayerInRange = false;

    void Start()
    {
        if (keyPromptObject != null)
        {
            keyPromptObject.SetActive(false);
        }
    }

    void Update()
    {
        if (isPlayerInRange)
        {
            bool pressedE = Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame;

            if (pressedE)
            {
                if (KeyManager.Instance != null && KeyManager.Instance.hasFullKey)
                {
                    if (audioSource != null && openSound != null)
                    {
                        audioSource.PlayOneShot(openSound);
                    }

                    if (!string.IsNullOrEmpty(targetSceneName))
                    {
                        SceneManager.LoadScene(targetSceneName);
                    }

                    if (keyPromptObject != null)
                    {
                        keyPromptObject.SetActive(false);
                    }
                }
                else
                {
                    if (audioSource != null && lockedSound != null)
                    {
                        audioSource.PlayOneShot(lockedSound);
                    }
                    Debug.Log("Puerta bloqueada. ¡Te faltan piezas de la llave!");
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (detectionArea != null && other != detectionArea && !other.IsTouching(detectionArea))
            return;

        PlayerController pc = other.GetComponentInParent<PlayerController>();
        if (pc != null)
        {
            isPlayerInRange = true;

            if (keyPromptObject != null)
            {
                keyPromptObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        PlayerController pc = other.GetComponentInParent<PlayerController>();
        if (pc != null)
        {
            isPlayerInRange = false;

            if (keyPromptObject != null)
            {
                keyPromptObject.SetActive(false);
            }
        }
    }
}