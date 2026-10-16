using UnityEngine;
using UnityEngine.InputSystem;

public class DoorInteraction : MonoBehaviour
{
    [SerializeField] private GameObject keyObject;
    [SerializeField] private Transform destinationPoint;
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
                    if (destinationPoint != null && playerTransform != null)
                    {
                        playerTransform.position = destinationPoint.position;
                    }

                    if (audioSource != null && openSound != null)
                    {
                        audioSource.PlayOneShot(openSound);
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