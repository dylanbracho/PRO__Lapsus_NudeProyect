using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private float strength = 5f;
    private float damage = 10f;

    void Start()
    {
        Destroy(gameObject, 4f);
    }

    public void SetStrength(float amount)
    {
        strength = amount;
        damage = amount;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController pc = other.GetComponent<PlayerController>();
        if (pc != null)
        {
            pc.hitTime = 0.5f;
            pc.hitForce = strength;
            pc.TakeDamage(damage);

            if (other.transform.position.x < transform.position.x)
            {
                pc.hitFromRight = true;
            }
            else
            {
                pc.hitFromRight = false;
            }

            Destroy(gameObject);
            return;
        }

        if (!other.CompareTag("Enemy") && !other.isTrigger)
        {
            Destroy(gameObject);
        }
    }
}