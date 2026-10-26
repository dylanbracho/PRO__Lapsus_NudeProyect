using UnityEngine;

public class whip : MonoBehaviour
{
    public int damage = 15;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1. Si es un enemigo normal, busca Health (mantiene todo lo demás funcionando)
        Health health = other.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(damage);
        }

        // 2. Si es la flor rosa del jefe, busca vidajefe
        vidajefe jefe = other.GetComponent<vidajefe>();
        if (jefe != null)
        {
            jefe.RecibirDaño(damage);
            Debug.Log("¡Látigo conectó con la flor rosa del jefe!");
        }
    }

}
