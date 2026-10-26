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
        //Health health = other.GetComponent<Health>();  VERSIÓN VIEJA, SI NO FUNCIONA PONGANLO Y BORREN LA NUEVA
        //if (health != null)
        //{
        //    health.TakeDamage(damage);
        //}

        if (other.TryGetComponent<Health>(out Health enemigo))
        {
            enemigo.TakeDamage(damage);
            //Debug.Log("cbdhsjj");
        }

        // 2. Si es la flor rosa del jefe, busca vidajefe

        if (other.TryGetComponent<nucleo>(out nucleo jefe))
        {
            jefe.RecibirDaño(damage);
            //Debug.Log("cbdhsjj");
        }
    }

}
