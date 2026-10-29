using UnityEngine;

public class DañoHitbox : MonoBehaviour
{
    [Header("Daño que hace este golpe")]
    [SerializeField] private float daño = 35f; // Daño del mazo

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Busca el script BossVida en el objeto que tocó o en sus padres/hijos
        BossVida vidaBoss = collision.GetComponentInParent<BossVida>();

        if (vidaBoss != null)
        {
            vidaBoss.RecibirDano(daño);
            Debug.Log($"¡Golpe exitoso del mazo! Le hizo {daño} de daño al jefe.");
        }
        else
        {
            Debug.Log($"El mazo tocó a: {collision.gameObject.name}, pero no tiene el script BossVida.");
        }
    }
}