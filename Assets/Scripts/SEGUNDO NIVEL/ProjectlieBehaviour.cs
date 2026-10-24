using UnityEngine;

public class ProjectlieBehaviour : MonoBehaviour
{
    public float speed = 10f;
    private Vector2 moveDirection = Vector2.right;
    public int damage = 10;

    void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }

    public void SetDirection(Vector2 newDirection)
    {
        moveDirection = newDirection;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Health enemyHealth = other.GetComponent<Health>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        if (!other.CompareTag("Player") && !other.isTrigger)
        {
            Destroy(gameObject);
        }
    }
}