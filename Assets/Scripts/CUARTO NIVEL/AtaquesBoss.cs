using System.Collections;
using UnityEngine;

public class AtaquesBoss : MonoBehaviour
{
    [SerializeField] private ParticleSystem sistemaSangre;

    [Header("Configuración de Cooldowns y Esperas")]
    [SerializeField] private float tiempoMinimoEntreAtaques = 1.5f;
    [SerializeField] private float tiempoMaximoEntreAtaques = 3.0f;

    private BasicMov movimientoJefe;
    private bool ejecutandoAtaque = false;
    private bool ataquesHabilitados = false; // No dispara hasta que la plataforma avise
    private int ultimoAtaqueElegido = -1;

    void Start()
    {
        movimientoJefe = GetComponent<BasicMov>();

        if (sistemaSangre == null)
        {
            sistemaSangre = transform.Find("Sangre")?.GetComponent<ParticleSystem>();
        }
    }

    // Llamado por la plataforma al llegar arriba
    public void ActivarAtaques()
    {
        if (ataquesHabilitados) return;
        ataquesHabilitados = true;
        StartCoroutine(BucleDeAtaques());
    }

    IEnumerator BucleDeAtaques()
    {
        // Espera de cortesía tras la presentación inicial del jefe antes de lanzar el primer chorro
        yield return new WaitForSeconds(5f);

        while (ataquesHabilitados)
        {
            if (movimientoJefe != null && !ejecutandoAtaque)
            {
                EjecutarAtaqueVariado();
            }

            float cooldownActual = Random.Range(tiempoMinimoEntreAtaques, tiempoMaximoEntreAtaques);
            yield return new WaitForSeconds(cooldownActual);
        }
    }

    private void EjecutarAtaqueVariado()
    {
        int ataqueElegido;

        do
        {
            ataqueElegido = Random.Range(0, 3);
        }
        while (ataqueElegido == ultimoAtaqueElegido);

        ultimoAtaqueElegido = ataqueElegido;

        switch (ataqueElegido)
        {
            case 0:
                StartCoroutine(RutinaChorroDirecto());
                break;
            case 1:
                StartCoroutine(RutinaRafagaTriple());
                break;
            case 2:
                StartCoroutine(RutinaAbanicoSangre());
                break;
        }
    }

    IEnumerator RutinaChorroDirecto()
    {
        ejecutandoAtaque = true;

        if (sistemaSangre != null && movimientoJefe.ObjetivoSara != null)
        {
            Vector3 posicionSara = movimientoJefe.ObjetivoSara.position;
            Vector3 posicionEmisor = sistemaSangre.transform.position;

            Vector2 direccion = (posicionSara - posicionEmisor).normalized;
            float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;

            sistemaSangre.transform.rotation = Quaternion.Euler(0f, 0f, angulo);
            sistemaSangre.Play();
        }

        yield return new WaitForSeconds(0.8f);
        ejecutandoAtaque = false;
    }

    IEnumerator RutinaRafagaTriple()
    {
        ejecutandoAtaque = true;

        for (int i = 0; i < 3; i++)
        {
            if (sistemaSangre != null && movimientoJefe.ObjetivoSara != null)
            {
                Vector3 posicionSara = movimientoJefe.ObjetivoSara.position;
                Vector3 posicionEmisor = sistemaSangre.transform.position;

                Vector2 direccion = (posicionSara - posicionEmisor).normalized;
                float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;

                sistemaSangre.transform.rotation = Quaternion.Euler(0f, 0f, angulo);
                sistemaSangre.Play();
            }

            yield return new WaitForSeconds(0.3f);
        }

        yield return new WaitForSeconds(0.6f);
        ejecutandoAtaque = false;
    }

    IEnumerator RutinaAbanicoSangre()
    {
        ejecutandoAtaque = true;

        if (sistemaSangre != null && movimientoJefe.ObjetivoSara != null)
        {
            Vector3 posicionSara = movimientoJefe.ObjetivoSara.position;
            Vector3 posicionEmisor = sistemaSangre.transform.position;

            Vector2 direccionBase = (posicionSara - posicionEmisor).normalized;
            float anguloBase = Mathf.Atan2(direccionBase.y, direccionBase.x) * Mathf.Rad2Deg;

            float[] offsetsAngulo = { -25f, 0f, 25f };

            foreach (float offset in offsetsAngulo)
            {
                sistemaSangre.transform.rotation = Quaternion.Euler(0f, 0f, anguloBase + offset);
                sistemaSangre.Play();
                yield return new WaitForSeconds(0.18f);
            }
        }

        yield return new WaitForSeconds(0.8f);
        ejecutandoAtaque = false;
    }
}