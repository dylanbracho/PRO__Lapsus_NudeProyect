using UnityEngine;

public class HeartLocketDoor : MonoBehaviour
{
    public AudioSource doorAudio;
    public AudioClip lockedSound;
    public AudioClip openSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>() != null || collision.name.Contains("Sara"))
        {
            if (KeyManager.Instance != null && KeyManager.Instance.hasFullKey)
            {
                if (doorAudio != null && openSound != null)
                    doorAudio.PlayOneShot(openSound);

                Debug.Log("Door Opened! Moving to Boss Scene...");
                gameObject.SetActive(false);
            }
            else
            {
                if (doorAudio != null && lockedSound != null)
                    doorAudio.PlayOneShot(lockedSound);

                Debug.Log("Door Locked! You need all 3 pieces.");
            }
        }
    }
}