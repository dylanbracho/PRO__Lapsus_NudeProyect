using System.Collections;
using UnityEngine;

public class AtaquesBoss : MonoBehaviour
{
    [SerializeField] private ParticleSystem sistemaSangre;

    [Header("Referencia al Jugador (Arrastra a Sara aquí desde el Inspector)")]
    [SerializeField] private Transform objetivoSeguimiento;

    [Header("Configuración de Cooldowns y Esperas")]
    [SerializeField] private float tiempoMinimoEntreAtaques = 1.5f;
    [SerializeField] private float tiempoMaximoEntreAtaques = 2.8f;

    [Header("Configuración de Lluvia sobre el Jugador (Box)")]
    [SerializeField] private float cajaAncho = 14f;
    [SerializeField] private float cajaAlto = 0.5f;
    [SerializeField] private float alturaSobreJugador = 8f;
    [SerializeField] private int cantidadOlasLluvia = 5;
    [SerializeField] private float intervaloEntreOlas = 0.6f;

    private BasicMov movimientoJefe;
    private bool ejecutandoAtaque = false;
    private bool ataquesHabilitados = false;
    private int ultimoAtaqueElegido = -1;

    void Start()
    {
        movimientoJefe = GetComponent<BasicMov>();

        if (objetivoSeguimiento == null && movimientoJefe != null && movimientoJefe.ObjetivoSara != null)
        {
            objetivoSeguimiento = movimientoJefe.ObjetivoSara;
        }

        if (sistemaSangre == null)
        {
            sistemaSangre = transform.Find("Sangre")?.GetComponent<ParticleSystem>();
        }
    }

    public void ActivarAtaques()
    {
        if (ataquesHabilitados) return;
        ataquesHabilitados = true;
        StartCoroutine(BucleDeAtaques());
    }

    IEnumerator BucleDeAtaques()
    {
        yield return new WaitForSeconds(3f);

        while (ataquesHabilitados)
        {
            if (!ejecutandoAtaque)
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
            int azar = Random.Range(0, 8);

            if (azar == 2 || azar == 5 || azar == 6)
            {
                ataqueElegido = 2; // Alta probabilidad de Lluvia
            }
            else if (azar == 0)
            {
                ataqueElegido = 0;
            }
            else if (azar == 1)
            {
                ataqueElegido = 1;
            }
            else if (azar == 3)
            {
                ataqueElegido = 3;
            }
            else
            {
                ataqueElegido = 4;
            }
        }
        while (ataqueElegido == ultimoAtaqueElegido);

        ultimoAtaqueElegido = ataqueElegido;

        switch (ataqueElegido)
        {
            case 0:
                StartCoroutine(RutinaDisparoLineal());
                break;
            case 1:
                StartCoroutine(RutinaMorteroArco());
                break;
            case 2:
                StartCoroutine(RutinaLluviaCaja());
                break;
            case 3:
                StartCoroutine(RutinaOndaDonut());
                break;
            case 4:
                StartCoroutine(RutinaPulsoCircular());
                break;
        }
    }

    // 1. DISPARO LINEAL
    IEnumerator RutinaDisparoLineal()
    {
        ejecutandoAtaque = true;

        if (sistemaSangre != null && objetivoSeguimiento != null)
        {
            var shape = sistemaSangre.shape;
            shape.shapeType = ParticleSystemShapeType.Circle;
            shape.radius = 0.05f;
            shape.scale = Vector3.one; // Asignación corregida vía variable temporal

            var main = sistemaSangre.main;
            main.startSpeed = 22f;
            main.startSize = 1.3f;
            main.gravityModifier = 0.25f;
            main.simulationSpace = ParticleSystemSimulationSpace.World;

            var emission = sistemaSangre.emission;
            emission.rateOverTime = 0;
            emission.SetBurst(0, new ParticleSystem.Burst(0f, 10f));

            Vector3 posicionObjetivo = objetivoSeguimiento.position;
            Vector3 posicionEmisor = sistemaSangre.transform.position;
            Vector2 direccion = (posicionObjetivo - posicionEmisor).normalized;
            float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;

            sistemaSangre.transform.localPosition = Vector3.zero;
            sistemaSangre.transform.rotation = Quaternion.Euler(0f, 0f, angulo);
            sistemaSangre.Play();
        }

        yield return new WaitForSeconds(1.6f);
        ejecutandoAtaque = false;
    }

    // 2. MORTERO EN ARCO
    IEnumerator RutinaMorteroArco()
    {
        ejecutandoAtaque = true;

        if (sistemaSangre != null && objetivoSeguimiento != null)
        {
            var shape = sistemaSangre.shape;
            shape.shapeType = ParticleSystemShapeType.Hemisphere;
            shape.radius = 0.8f;
            shape.scale = Vector3.one;

            var main = sistemaSangre.main;
            main.startSpeed = 15f;
            main.startSize = 1.4f;
            main.gravityModifier = 1.3f;
            main.simulationSpace = ParticleSystemSimulationSpace.World;

            var emission = sistemaSangre.emission;
            emission.rateOverTime = 0;
            emission.SetBurst(0, new ParticleSystem.Burst(0f, 12f));

            Vector3 posicionObjetivo = objetivoSeguimiento.position;
            Vector3 posicionEmisor = sistemaSangre.transform.position;
            Vector2 direccion = (posicionObjetivo - posicionEmisor).normalized;

            float anguloBase = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
            float anguloMortero = Mathf.Max(anguloBase, 45f);

            sistemaSangre.transform.localPosition = Vector3.zero;
            sistemaSangre.transform.rotation = Quaternion.Euler(0f, 0f, anguloMortero);
            sistemaSangre.Play();
        }

        yield return new WaitForSeconds(2.2f);
        ejecutandoAtaque = false;
    }

    // 3. LLUVIA EN CAJA
    IEnumerator RutinaLluviaCaja()
    {
        ejecutandoAtaque = true;

        if (sistemaSangre != null && objetivoSeguimiento != null)
        {
            var shape = sistemaSangre.shape;
            shape.shapeType = ParticleSystemShapeType.Box;
            shape.scale = new Vector3(cajaAlto, cajaAncho, 1f); // Escala exclusiva para Box

            var main = sistemaSangre.main;
            main.startSpeed = 2.5f;
            main.startSize = 1.3f;
            main.gravityModifier = 1.2f;
            main.simulationSpace = ParticleSystemSimulationSpace.World;

            var emission = sistemaSangre.emission;
            emission.rateOverTime = 0;

            Vector3 posObjetivo = objetivoSeguimiento.position;
            sistemaSangre.transform.position = new Vector3(posObjetivo.x, posObjetivo.y + alturaSobreJugador, posObjetivo.z);
            sistemaSangre.transform.rotation = Quaternion.Euler(0f, 0f, -90f);

            for (int i = 0; i < cantidadOlasLluvia; i++)
            {
                emission.SetBurst(0, new ParticleSystem.Burst(0f, 35f));
                sistemaSangre.Play();

                yield return new WaitForSeconds(intervaloEntreOlas);
            }
        }

        yield return new WaitForSeconds(0.8f);

        sistemaSangre.transform.localPosition = Vector3.zero;
        sistemaSangre.transform.localRotation = Quaternion.identity;

        var mainRest = sistemaSangre.main;
        mainRest.simulationSpace = ParticleSystemSimulationSpace.Local;

        var shapeRest = sistemaSangre.shape;
        shapeRest.scale = Vector3.one; // Restablece la escala por defecto

        ejecutandoAtaque = false;
    }

    // 4. ONDA DONUT
    IEnumerator RutinaOndaDonut()
    {
        ejecutandoAtaque = true;

        if (sistemaSangre != null && objetivoSeguimiento != null)
        {
            var shape = sistemaSangre.shape;
            shape.shapeType = ParticleSystemShapeType.Donut;
            shape.radius = 1.8f;
            shape.donutRadius = 0.5f;
            shape.scale = Vector3.one;

            var main = sistemaSangre.main;
            main.startSpeed = 11f;
            main.startSize = 1.3f;
            main.gravityModifier = 0.25f;
            main.simulationSpace = ParticleSystemSimulationSpace.World;

            var emission = sistemaSangre.emission;
            emission.rateOverTime = 0;
            emission.SetBurst(0, new ParticleSystem.Burst(0f, 35f));

            Vector3 posicionObjetivo = objetivoSeguimiento.position;
            Vector3 posicionEmisor = sistemaSangre.transform.position;
            Vector2 direccion = (posicionObjetivo - posicionEmisor).normalized;
            float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;

            sistemaSangre.transform.localPosition = Vector3.zero;
            sistemaSangre.transform.rotation = Quaternion.Euler(0f, 0f, angulo);
            sistemaSangre.Play();
        }

        yield return new WaitForSeconds(2.0f);
        ejecutandoAtaque = false;
    }

    // 5. PULSO CIRCULAR
    IEnumerator RutinaPulsoCircular()
    {
        ejecutandoAtaque = true;

        if (sistemaSangre != null && objetivoSeguimiento != null)
        {
            var shape = sistemaSangre.shape;
            shape.shapeType = ParticleSystemShapeType.Circle;
            shape.radius = 0.8f;
            shape.scale = Vector3.one;

            var main = sistemaSangre.main;
            main.startSpeed = 12f;
            main.startSize = 1.3f;
            main.gravityModifier = 0.2f;
            main.simulationSpace = ParticleSystemSimulationSpace.World;

            var emission = sistemaSangre.emission;
            emission.rateOverTime = 0;
            emission.SetBurst(0, new ParticleSystem.Burst(0f, 6f));

            Vector3 posicionObjetivo = objetivoSeguimiento.position;
            Vector3 posicionEmisor = sistemaSangre.transform.position;
            Vector2 direccion = (posicionObjetivo - posicionEmisor).normalized;
            float anguloBase = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;

            int pasos = 6;
            float pasoAngulo = 360f / pasos;

            sistemaSangre.transform.localPosition = Vector3.zero;

            for (int i = 0; i < pasos; i++)
            {
                float anguloActual = anguloBase + (i * pasoAngulo);
                sistemaSangre.transform.rotation = Quaternion.Euler(0f, 0f, anguloActual);
                sistemaSangre.Play();
                yield return new WaitForSeconds(0.1f);
            }
        }

        yield return new WaitForSeconds(2.0f);
        ejecutandoAtaque = false;
    }
}