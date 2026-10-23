using UnityEngine;

public class DanoParticulas : MonoBehaviour
{
    [Header("Configuración de Daño")]
    [SerializeField] private float danoPorParticula = 5f;

    private void OnParticleCollision(GameObject other)
    {
        // Esto se imprime pase lo que pase cuando una partícula toque algo
        Debug.Log(">>> [DIAGNÓSTICO] ¡Una partícula chocó con: " + other.name + " (Tag: " + other.tag + ")");

        // Buscamos el script en el objeto o sus padres sin importar el tag por ahora
      
        PlayerController player = other.GetComponent<PlayerController>();

        if (player != null)
        {
            Debug.Log(">>> [DIAGNÓSTICO] ¡PlayerController encontrado en: " + player.gameObject.name + "!");
            Debug.Log(">>> [DIAGNÓSTICO] Vida antes del daño: " + player.health);

            player.TakeDamage(danoPorParticula);
            Debug.Log("JuanPBandido");

            Debug.Log(">>> [DIAGNÓSTICO] Vida después del daño: " + player.health);
        }
        else
        {
            Debug.LogWarning(">>> [DIAGNÓSTICO] ¡ALERTA! Chocó con " + other.name + ", pero NO se encontró el script PlayerController en él ni en sus padres.");
        }
    }
}