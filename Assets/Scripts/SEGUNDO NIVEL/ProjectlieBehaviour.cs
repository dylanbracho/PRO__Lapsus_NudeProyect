using UnityEngine;

public class ProjectlieBehaviour : MonoBehaviour
{
    

    public float speed = 5f;
    private Vector2 direction = Vector2.right;
    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (direction.x < 0 ? -1 : 1);
        transform.localScale = scale;
    }
    private void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }
 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(10f); // Adjust the damage value as needed
            }
        }
    }
}
