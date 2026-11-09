using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class EntradaJefe : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private GameObject jefe;
    [SerializeField] private CinemachineCamera camaraSeguimientoSara;

    [Header("Configuración de Zoom y Encuadre")]
    [SerializeField] private float nuevoOrthoSize = 20f;
    [Tooltip("Sube este valor si quieres que la cámara apunte más arriba y tape el piso de abajo.")]
    [SerializeField] private float offsetCamaraY = 3f; // Ajusta este número en el Inspector a tu gusto

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
                // Aplicamos el Zoom
                camaraSeguimientoSara.Lens.OrthographicSize = nuevoOrthoSize;

                // Buscamos el componente PositionComposer de Cinemachine para modificar el Target Offset de forma segura
                CinemachinePositionComposer compositor = camaraSeguimientoSara.GetComponent<CinemachinePositionComposer>();
                if (compositor != null)
                {
                    Vector3 offsetActual = compositor.TargetOffset;
                    compositor.TargetOffset = new Vector3(offsetActual.x, offsetCamaraY, offsetActual.z);
                }

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

            Vector2 desplazamientoAleatorio = Random.insideUnitCircle * intensidadShake;
            transformCamara.localPosition = new Vector3(posicionBase.x + desplazamientoAleatorio.x, posicionBase.y + desplazamientoAleatorio.y, posicionBase.z);

            yield return null;
        }

        transformCamara.localPosition = posicionBase;
    }
}