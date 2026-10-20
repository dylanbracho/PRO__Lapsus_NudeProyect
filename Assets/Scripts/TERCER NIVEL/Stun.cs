using System.Collections;
using UnityEngine;

public class Stun : MonoBehaviour
{
    public PlayerController player;

    private void Start()
    {
        if (player == null)
        {
            player = FindObjectOfType<PlayerController>();
        }
    }

    IEnumerator StunPlayer(float duration)
    {
        player.enabled = false;
        player.canJump = false;
        player.canDash = false;
        Debug.Log("Player is stunned for " + duration + " seconds.");
        yield return new WaitForSeconds(duration);
        Debug.Log("Player is no longer stunned.");
        player.enabled = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        
       StartCoroutine(StunPlayer(2f));
        
    }

    
}
