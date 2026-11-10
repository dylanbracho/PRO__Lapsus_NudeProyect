using UnityEngine;

public class DanoParticulas : MonoBehaviour
{
    [Header("Configuración de Daño")]
    [SerializeField] private float danoPorParticula = 5f;
    [SerializeField] private vidajefe4 jefeScript; // Referencia al script de vida del jefe

    private void Start()
    {
        // Si no lo asignaste en el Inspector, usa la función moderna recomendada por Unity
        if (jefeScript == null)
        {
            jefeScript = Object.FindFirstObjectByType<vidajefe4>();
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("OnParticleCollision disparado con: " + other.name);

        if (other.TryGetComponent<PlayerController>(out PlayerController player1))
        {
            Debug.Log("SÍ tiene PlayerController");

            // 1. Sara recibe el daño completo de las partículas
            player1.TakeDamage(danoPorParticula);

            // 2. vidajefe4 absorbe y se cura el 50% exacto de ese daño infligido a Sara
            if (jefeScript != null)
            {
                float vidaRobada = danoPorParticula * 0.5f;
                jefeScript.Curar(vidaRobada);
            }
        }
        else
        {
            Debug.Log("NO tiene PlayerController, el objeto que choco se llama: " + other.name);
        }
    }
}