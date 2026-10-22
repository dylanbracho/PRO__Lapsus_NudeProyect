using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private int damage = 10;

    public void SetDamage(int amount)
    {
        damage = amount;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController pc = other.GetComponentInParent<PlayerController>();
        if (pc != null)
        {
            Destroy(gameObject);
        }
    }
}