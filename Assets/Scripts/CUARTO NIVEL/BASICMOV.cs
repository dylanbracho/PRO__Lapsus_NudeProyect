using System.Collections;
using UnityEngine;

public class BasicMov : MonoBehaviour
{
    [SerializeField] private Transform objetivoSara;

    public Vector2[] puntosDeReposo = new Vector2[]
    {
        new Vector2(-40f, 8f),
        new Vector2(-10f, 8f),
        new Vector2(25f, 8f)
    };

    [SerializeField] private float velocidadViaje = 9f;
    [SerializeField] private float amplitudFlote = 10f;
    [SerializeField] private float ritmoFlote = 2f;

    private bool jefeActivado = false;
    private bool enSecuenciaInicio = false;
    private bool pausadoEnPunto = false;
    [HideInInspector] public bool estaEnReposado = false;
    [HideInInspector] public bool movimientoPausado = false;

    private int indicePuntoActual = 0;
    private Vector3 centroActual;
    private Vector3 centroObjetivo;

    public int IndicePuntoActual => indicePuntoActual;
    public bool PausadoEnPunto => pausadoEnPunto;
    public Transform ObjetivoSara => objetivoSara; // Nueva propiedad expuesta

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

        centroActual = transform.position;
        centroObjetivo = transform.position;
    }

    void Update()
    {
        if (!jefeActivado || movimientoPausado) return;

        if (enSecuenciaInicio)
        {
            float tiempoTemblor = Time.time * 30f;
            float temblorX = Mathf.Sin(tiempoTemblor) * 0.3f;
            float temblorY = Mathf.Cos(tiempoTemblor * 1.3f) * 0.3f;
            transform.position = centroActual + new Vector3(temblorX, temblorY, 0f);
            return;
        }

        if (!pausadoEnPunto)
        {
            centroActual = Vector3.MoveTowards(centroActual, centroObjetivo, velocidadViaje * Time.deltaTime);
        }

        float tiempo = Time.time * ritmoFlote;
        float offsetX = Mathf.Sin(tiempo) * amplitudFlote;
        float offsetY = Mathf.Cos(tiempo * 1.5f) * (amplitudFlote * 0.7f);

        transform.position = centroActual + new Vector3(offsetX, offsetY, 0f);

        Vector3 direccionMovimiento = (centroObjetivo - centroActual);
        if (direccionMovimiento.sqrMagnitude > 0.01f)
        {
            float inclinacionZ = Mathf.Sin(tiempo * 2f) * 8f;
            transform.rotation = Quaternion.Euler(0f, 0f, inclinacionZ);
        }

        if (!pausadoEnPunto && Vector3.Distance(centroActual, centroObjetivo) < 1.5f)
        {
            StartCoroutine(PausaEnPuntoRutina());
        }

        estaEnReposado = pausadoEnPunto;
    }

    public void ActivarJefe()
    {
        if (jefeActivado) return;

        jefeActivado = true;
        centroActual = transform.position;
        centroObjetivo = transform.position;

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
        ElegirSiguientePunto();
        StartCoroutine(RutinaPatronPuntos());
    }

    IEnumerator PausaEnPuntoRutina()
    {
        pausadoEnPunto = true;
        yield return new WaitForSeconds(2f);
        pausadoEnPunto = false;
        ElegirSiguientePunto();
    }

    IEnumerator RutinaPatronPuntos()
    {
        while (true)
        {
            yield return null;
        }
    }

    private void ElegirSiguientePunto()
    {
        if (puntosDeReposo == null || puntosDeReposo.Length == 0) return;

        int nuevoIndice = Random.Range(0, puntosDeReposo.Length);
        if (nuevoIndice == indicePuntoActual && puntosDeReposo.Length > 1)
        {
            nuevoIndice = (indicePuntoActual + 1) % puntosDeReposo.Length;
        }

        indicePuntoActual = nuevoIndice;
        centroObjetivo = puntosDeReposo[indicePuntoActual];
    }

    private void OnDrawGizmosSelected()
    {
        if (puntosDeReposo == null) return;
        Gizmos.color = Color.cyan;
        foreach (Vector2 punto in puntosDeReposo)
        {
            Gizmos.DrawWireSphere(punto, 2f);
        }
    }
}