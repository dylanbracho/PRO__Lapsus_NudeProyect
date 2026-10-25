using UnityEngine;

public class ProjectlieBehaviour : MonoBehaviour
{
    public float speed = 10f;
    private Vector2 moveDirection = Vector2.right;
    public float damage = 10f;
    [SerializeField]bool enemyProjectile = false;

    private void Start()
    {
       
    }

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

        if (other.CompareTag("Player") && enemyProjectile)
        {
            PlayerController playerHealth = other.GetComponentInParent<PlayerController>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}