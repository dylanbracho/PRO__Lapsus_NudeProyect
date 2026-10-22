using System.Collections;
using UnityEngine;
using Unity.Cinemachine;

public class EntradaJefe : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private GameObject jefe;
    [SerializeField] private CinemachineCamera camaraSeguimientoSara;

    [Header("Configuración de Zoom de Arena")]
    [SerializeField] private float nuevoOrthoSize = 20f;

    [Header("Configuración de Microshake")]
    [SerializeField] private float duracionShake = 2f;
    [SerializeField] private float intensidadShake = 0.15f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (jefe != null)
            {
                BasicMov scriptMov = jefe.GetComponent<BasicMov>();
                if (scriptMov != null)
                {
                    scriptMov.ActivarJefe();
                }
            }

            if (camaraSeguimientoSara != null)
            {
                camaraSeguimientoSara.Lens.OrthographicSize = nuevoOrthoSize;
                StartCoroutine(RutinaMicroshake());
            }
            else
            {
                Debug.LogWarning("La referencia de 'Camara Seguimiento Sara' está vacía en el Inspector.");
            }

            gameObject.SetActive(false);
        }
    }

    IEnumerator RutinaMicroshake()
    {
        Transform transformCamara = camaraSeguimientoSara.transform;
        Vector3 posicionBase = transformCamara.localPosition;
        float tiempoTranscurrido = 0f;

        while (tiempoTranscurrido < duracionShake)
        {
            tiempoTranscurrido += Time.deltaTime;

            // Genera un temblor sutil y rápido de sorpresa
            Vector2 desplazamientoAleatorio = Random.insideUnitCircle * intensidadShake;
            transformCamara.localPosition = new Vector3(posicionBase.x + desplazamientoAleatorio.x, posicionBase.y + desplazamientoAleatorio.y, posicionBase.z);

            yield return null;
        }

        // Restablece la posición exacta al terminar
        transformCamara.localPosition = posicionBase;
    }
}