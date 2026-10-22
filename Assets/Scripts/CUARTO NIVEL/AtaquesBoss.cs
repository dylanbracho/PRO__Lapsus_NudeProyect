using System.Collections;
using UnityEngine;

public class AtaquesBoss : MonoBehaviour
{
    [SerializeField] private ParticleSystem sistemaSangre;

    [Header("Configuración de Estampida")]
    [SerializeField] private float duracionCarga = 0.8f;
    [SerializeField] private float duracionAtaque = 0.25f;
    [SerializeField] private float distanciaRetroceso = 1.5f;
    [SerializeField] private float distanciaOvershoot = 4f;

    private BasicMov movimientoJefe;
    private bool ataqueEjecutadoEnEstaPausa = false;

    void Start()
    {
        movimientoJefe = GetComponent<BasicMov>();

        if (sistemaSangre == null)
        {
            sistemaSangre = transform.Find("Sangre")?.GetComponent<ParticleSystem>();
        }
    }

    void Update()
    {
        if (movimientoJefe == null) return;

        if (movimientoJefe.PausadoEnPunto)
        {
            if (!ataqueEjecutadoEnEstaPausa)
            {
                ataqueEjecutadoEnEstaPausa = true;
                EjecutarAtaqueAleatorio();
            }
        }
        else
        {
            ataqueEjecutadoEnEstaPausa = false;
        }
    }

    private void EjecutarAtaqueAleatorio()
    {
        int ataqueElegido = Random.Range(0, 3);

        switch (ataqueElegido)
        {
            case 0:
                DispararSangre();
                break;
            case 1:
                StartCoroutine(RutinaEstampida());
                break;
            case 2:
                LanzarTercerAtaque();
                break;
        }
    }

    private void DispararSangre()
    {
        if (sistemaSangre != null && movimientoJefe.ObjetivoSara != null)
        {
            Vector3 posicionSara = movimientoJefe.ObjetivoSara.position;
            Vector3 posicionEmisor = sistemaSangre.transform.position;

            Vector2 direccion = (posicionSara - posicionEmisor).normalized;

            float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;

            sistemaSangre.transform.rotation = Quaternion.Euler(0f, 0f, angulo - 90f);

            sistemaSangre.Play();
        }
    }

    IEnumerator RutinaEstampida()
    {
        if (movimientoJefe.ObjetivoSara == null) yield break;

        movimientoJefe.movimientoPausado = true;

        Vector3 posicionInicial = transform.position;
        Vector3 posicionSara = movimientoJefe.ObjetivoSara.position;
        Vector3 direccionCarga = (posicionSara - posicionInicial).normalized;

        Vector3 puntoLentitud = posicionInicial - (direccionCarga * distanciaRetroceso);
        Vector3 puntoDestino = posicionSara + (direccionCarga * distanciaOvershoot);

        float tiempoCarga = 0f;
        while (tiempoCarga < duracionCarga)
        {
            tiempoCarga += Time.deltaTime;
            float progreso = tiempoCarga / duracionCarga;
            float curvaSuave = Mathf.SmoothStep(0f, 1f, progreso);

            transform.position = Vector3.Lerp(posicionInicial, puntoLentitud, curvaSuave);

            float angulo = Mathf.Atan2(direccionCarga.y, direccionCarga.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 0f, angulo), Time.deltaTime * 10f);

            yield return null;
        }

        float tiempoAtaque = 0f;
        while (tiempoAtaque < duracionAtaque)
        {
            tiempoAtaque += Time.deltaTime;
            float progreso = tiempoAtaque / duracionAtaque;

            transform.position = Vector3.Lerp(puntoLentitud, puntoDestino, progreso);

            yield return null;
        }

        yield return new WaitForSeconds(0.4f);

        transform.rotation = Quaternion.identity;
        movimientoJefe.movimientoPausado = false;
    }

    private void LanzarTercerAtaque()
    {
        StartCoroutine(RutinaLluviaHorizontal());
    }

    IEnumerator RutinaLluviaHorizontal()
    {
        if (movimientoJefe == null || movimientoJefe.ObjetivoSara == null) yield break;

        movimientoJefe.movimientoPausado = true;

        float direccionBarrido = Mathf.Sign(movimientoJefe.ObjetivoSara.position.x - transform.position.x);
        Vector3 posicionInicial = transform.position;
        Vector3 puntoDestino = posicionInicial + new Vector3(direccionBarrido * 12f, 0f, 0f);

        float tiempoViaje = 0f;
        float duracionBarrido = 4f;
        float intervaloDisparo = 0.3f;
        float temporizadorDisparo = 0f;

        while (tiempoViaje < duracionBarrido)
        {
            tiempoViaje += Time.deltaTime;
            temporizadorDisparo += Time.deltaTime;

            float progreso = tiempoViaje / duracionBarrido;
            transform.position = Vector3.Lerp(posicionInicial, puntoDestino, progreso);

            if (temporizadorDisparo >= intervaloDisparo)
            {
                temporizadorDisparo = 0f;

                if (sistemaSangre != null)
                {
                    sistemaSangre.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
                    sistemaSangre.Play();
                }
            }

            yield return null;
        }

        yield return new WaitForSeconds(0.4f);

        transform.rotation = Quaternion.identity;
        movimientoJefe.movimientoPausado = false;
    }
}