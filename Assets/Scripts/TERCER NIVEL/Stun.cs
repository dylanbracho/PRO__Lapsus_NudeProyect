using UnityEngine;

public class Stun : MonoBehaviour
{
    [SerializeField] private float stunDuration = 1f;

    void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player != null)
        {
            player.Stun(stunDuration);
        }
    }
}