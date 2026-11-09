using System.Collections;
using UnityEngine;

public class BasicMov : MonoBehaviour
{
    [SerializeField] private Transform objetivoSara;

    [Header("Configuración de Palpitación / Respiración")]
    [SerializeField] private float velocidadPalpitacion = 4f;
    [SerializeField] private float intensidadPalpitacion = 0.3f;

    private bool jefeActivado = false;
    private bool enSecuenciaInicio = false;
    [HideInInspector] public bool estaEnReposado = false;
    [HideInInspector] public bool movimientoPausado = false;

    private Vector3 posicionFija;

    public Transform ObjetivoSara => objetivoSara;

    void Start()
    {
        if (objetivoSara == null)
        {
            GameObject saraObj = GameObject.FindGameObjectWithTag("Player");
            if (saraObj != null)
            {
                objetivoSara = saraObj.transform;
            }
        }

        posicionFija = transform.position;
    }

    void Update()
    {
        // Si no ha sido activado por la plataforma, no hace nada
        if (!jefeActivado || movimientoPausado) return;

        if (enSecuenciaInicio)
        {
            float tiempoTemblor = Time.time * 30f;
            float temblorX = Mathf.Sin(tiempoTemblor) * 0.3f;
            float temblorY = Mathf.Cos(tiempoTemblor * 1.3f) * 0.3f;
            transform.position = posicionFija + new Vector3(temblorX, temblorY, 0f);
            return;
        }

        // Efecto de palpitación sutil en su lugar fijo
        float tiempo = Time.time * velocidadPalpitacion;
        float offsetX = Mathf.Sin(tiempo) * intensidadPalpitacion;
        float offsetY = Mathf.Cos(tiempo * 1.5f) * (intensidadPalpitacion * 0.7f);

        transform.position = posicionFija + new Vector3(offsetX, offsetY, 0f);

        float inclinacionZ = Mathf.Sin(tiempo * 0.5f) * 2f;
        transform.rotation = Quaternion.Euler(0f, 0f, inclinacionZ);

        estaEnReposado = true;
    }

    public void ActivarJefe()
    {
        if (jefeActivado) return;

        jefeActivado = true;
        posicionFija = transform.position;

        StartCoroutine(RutinaSecuenciaInicial());
    }

    IEnumerator RutinaSecuenciaInicial()
    {
        enSecuenciaInicio = true;

        yield return new WaitForSeconds(3f);

        float tiempoInicioTemblor = Time.time;
        while (Time.time - tiempoInicioTemblor < 2f)
        {
            yield return null;
        }

        enSecuenciaInicio = false;
    }
}