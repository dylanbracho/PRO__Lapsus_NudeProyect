using System.Collections;
using UnityEngine;

public class FallPlatform : MonoBehaviour
{
    [SerializeField] private float fallDelay = 0.8f;     // Time standing on it before it falls
    [SerializeField] private float respawnDelay = 3f;    // Time after falling before it returns
    [SerializeField] private float fallGravity = 10f;

    private Rigidbody2D rb;
    private Vector3 startPosition;
    private bool isFalling;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isFalling && collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(FallAndRespawn());
        }
    }

    IEnumerator FallAndRespawn()
    {
        isFalling = true;

        yield return new WaitForSeconds(fallDelay);

        // Fall
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = fallGravity;

        yield return new WaitForSeconds(respawnDelay);

        // Respawn: stop all motion, make it static again, and move it back
        rb.linearVelocity = Vector2.zero;   // use rb.velocity on older Unity versions
        rb.angularVelocity = 0f;
        rb.bodyType = RigidbodyType2D.Kinematic;
        transform.position = startPosition;

        isFalling = false;
    }
}