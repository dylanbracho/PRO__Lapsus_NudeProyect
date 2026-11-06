using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SignifierDialogo : MonoBehaviour
{
    [SerializeField] private Collider2D detectionArea;
    private bool isPlayerInRange = false;
    private Transform playerTransform;
    [SerializeField] private GameObject keyObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (keyObject != null)
        {
            keyObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerInRange)
        {
            bool pressedE = Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame;

            if (pressedE)
            {
                
                    if (keyObject != null)
                    {
                        keyObject.SetActive(false);
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
