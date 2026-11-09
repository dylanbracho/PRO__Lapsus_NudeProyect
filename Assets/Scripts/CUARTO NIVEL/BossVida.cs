using UnityEngine;

public class BossVida : MonoBehaviour
{
    [Header("Configuración de Vida")]
    public float maxhealth = 100f;
    public float health;

    void Start()
    {
        // Inicializamos la vida al arrancar
        health = maxhealth;
    }

    void Update()
    {
        // Si la vida llega a 0, se desactiva el jefe
        if (health <= 0)
        {
            Debug.Log("¡El Heart Enemy ha sido derrotado!");
            gameObject.SetActive(false);
        }
    }

    // Usamos TakeDamage para que coincida con la estructura de tu PlayerController
    public void TakeDamage(float cantidad)
    {
        health -= cantidad;
        health = Mathf.Clamp(health, 0, maxhealth);

        Debug.Log($"¡Heart Enemy recibió {cantidad} de daño! Vida actual: {health}/{maxhealth}");
    }
}