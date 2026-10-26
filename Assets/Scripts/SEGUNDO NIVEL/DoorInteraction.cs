using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement; // Necesario para cambiar de escena

public class DoorInteraction : MonoBehaviour
{
    [SerializeField] private GameObject keyObject;
    [SerializeField] private string targetSceneName; // Nombre de la otra escena a la que quieres viajar
    [SerializeField] private bool isDoorOpen = false;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip lockedSound;
    [SerializeField] private Collider2D detectionArea;

    private bool isPlayerInRange = false;
    private Transform playerTransform;

    void Start()
    {
        if (keyObject != null)
        {
            keyObject.SetActive(false);
        }
    }

    void Update()
    {
        if (isPlayerInRange)
        {
            bool pressedE = Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame;

            if (pressedE)
            {
                if (isDoorOpen)
                {
                    if (audioSource != null && openSound != null)
                    {
                        audioSource.PlayOneShot(openSound);
                    }

                    // Cambia a la escena especificada en el Inspector
                    if (!string.IsNullOrEmpty(targetSceneName))
                    {
                        SceneManager.LoadScene(targetSceneName);
                    }

                    if (keyObject != null)
                    {
                        keyObject.SetActive(false);
                    }
                }
                else
                {
                    if (audioSource != null && lockedSound != null)
                    {
                        audioSource.PlayOneShot(lockedSound);
                    }
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
            playerTransform = pc.transform;

            if (keyObject != null)
            {
                keyObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        PlayerController pc = other.GetComponentInParent<PlayerController>();
        if (pc != null)
        {
            isPlayerInRange = false;
            playerTransform = null;

            if (keyObject != null)
            {
                keyObject.SetActive(false);
            }
        }
    }
}