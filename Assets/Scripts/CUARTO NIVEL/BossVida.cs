using System.Collections;
using UnityEngine;

public class BossVida : MonoBehaviour
{
    [Header("Configuración de Vida")]
    [SerializeField] private float vidaMaxima = 100f;
    private float vidaActual;

    [Header("Efectos o Acciones al Morir")]
    [SerializeField] private GameObject efectoMuerte; 

    private bool estaMuerto = false;

    void Start()
    {
        vidaActual = vidaMaxima;
    }


    public void RecibirDano(float cantidad)
    {
        if (estaMuerto) return;

        vidaActual -= cantidad;
        vidaActual = Mathf.Clamp(vidaActual, 0f, vidaMaxima);

        Debug.Log($"¡El Jefe recibió {cantidad} de daño! Vida restante: {vidaActual}/{vidaMaxima}");

        if (vidaActual <= 0f)
        {
            Morir();
        }
    }

    private void Morir()
    {
        estaMuerto = true;
        Debug.Log("¡El Jefe ha sido derrotado!");

        if (TryGetComponent<AtaquesBoss>(out AtaquesBoss ataques)) ataques.enabled = false;
        if (TryGetComponent<BasicMov>(out BasicMov movimiento)) movimiento.enabled = false;

        foreach (Collider2D col in GetComponentsInChildren<Collider2D>())
        {
            col.enabled = false;
        }

        if (efectoMuerte != null)
        {
            efectoMuerte.SetActive(true);
        }
        StartCoroutine(RutinaMuerte());
    }

    IEnumerator RutinaMuerte()
    {
        yield return new WaitForSeconds(2f);

        Destroy(gameObject);
    }
}