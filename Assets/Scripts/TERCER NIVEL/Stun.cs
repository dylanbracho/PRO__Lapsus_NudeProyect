using System.Collections;
using UnityEngine;

public class Stun : MonoBehaviour
{
    public PlayerController player;

    IEnumerator StunPlayer(float duration)
    {
        player.enabled = false;
        player.canJump = false;
        Debug.Log("Player is stunned for " + duration + " seconds.");
        yield return new WaitForSeconds(duration);
        Debug.Log("Player is no longer stunned.");
        player.enabled = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        
       StartCoroutine(StunPlayer(5f));
        
    }

    
}
