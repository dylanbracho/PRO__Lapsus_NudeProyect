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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
