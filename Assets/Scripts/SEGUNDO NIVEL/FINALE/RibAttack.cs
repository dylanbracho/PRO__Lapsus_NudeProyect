using UnityEngine;

public class RibAttack : MonoBehaviour
{
    public float damage = 20f;
    public float lifetime = 1.5f; // Cuánto dura en pantalla antes de destruirse
    public float moveSpeed = 5f;  // Si quieres que se mueva hacia adelante al sacarla
    private bool moveRight = true;

    void Start()
    {
        // Se destruye automáticamente después de un tiempo para no acumular basura en memoria
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Hace que la costilla se desplace un poco hacia el frente al ser "sacada"
        float direction = moveRight ? 1f : -1f;
        transform.Translate(Vector3.right * direction * moveSpeed * Time.deltaTime);
    }

    // Método para orientarla según hacia dónde mire la Prom Queen
    public void SetDirection(bool facingRight)
    {
        moveRight = facingRight;
        if (!moveRight)
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si la costilla toca a Sara (Player), le hace daño y desaparece
        PlayerController pc = collision.GetComponentInParent<PlayerController>();
        if (pc != null)
        {
            pc.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}