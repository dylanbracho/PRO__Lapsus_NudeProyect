using System.Collections;
using UnityEngine;

public class FallPlatform : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Start the coroutine to make the platform fall
            StartCoroutine(Fall());
        }
    }

    IEnumerator Fall()
    {
        // Wait for a short delay before making the platform fall
        yield return new WaitForSeconds(0.8f);
        // Make the platform fall by enabling gravity and disabling kinematic
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.gravityScale = 10f; // Adjust gravity scale as needed
        }
    }
}
