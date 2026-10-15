using System.Collections;
using UnityEngine;

public class BossCorazon : MonoBehaviour
{
    [Header("Objetivo")]
    [SerializeField] private Transform objetivoSara;

    [Header("Movimiento de Merodeo")]
    [SerializeField] private float distanciaMinimaSeguridad = 50f;
    [SerializeField] private float velocidadMovimiento = 0.4f;
    [SerializeField] private float suavidadMovimiento = 2f;

    [Header("Límites del Escenario")]
    [SerializeField] private float minX = -40f;
    [SerializeField] private float maxX = 40f;
    [SerializeField] private float minY = -10f;
    [SerializeField] private float maxY = 35f;

    [Header("Control de Ataques y Cooldowns")]
    [SerializeField] private float tiempoAviso = 0.8f;
    [SerializeField] private float velocidadAtaque = 38f;
    [SerializeField] private float minCooldown = 3.0f;
    [SerializeField] private float maxCooldown = 5.5f;

    private SpriteRenderer spriteRenderer;
    private Color colorOriginal;
    private bool estaAtacando = false;
    private Vector3 velocidadSmoothDamp;
    private Vector3 escalaOriginal;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null) colorOriginal = spriteRenderer.color;
        escalaOriginal = transform.localScale;

        if (objetivoSara == null)
        {
            GameObject saraObj = GameObject.FindGameObjectWithTag("Player");
            if (saraObj != null)
            {
                objetivoSara = saraObj.transform;
            }
        }

        StartCoroutine(BuclePrincipalIA());
        StartCoroutine(RutinaLatidoEscala());
    }

    void Update()
    {
        if (objetivoSara == null) return;

        if (!estaAtacando)
        {
            MoverseLejosDelJugador();
        }

        AplicarLimitesEscenario();
    }

    // Movimiento flotante de patrulla manteniendo distancia segura
    void MoverseLejosDelJugador()
    {
        float tiempo = Time.time * velocidadMovimiento;

        float angulo = Mathf.PerlinNoise(tiempo, 0.3f) * Mathf.PI * 2f;
        float distancia = distanciaMinimaSeguridad + (Mathf.PerlinNoise(0.3f, tiempo) * 20f);

        float posX = objetivoSara.position.x + Mathf.Cos(angulo) * distancia;
        float posY = objetivoSara.position.y + Mathf.Sin(angulo) * (distancia * 0.4f) + 15f;

        Vector3 posicionDeseada = new Vector3(posX, posY, transform.position.z);

        Vector3 separacion = posicionDeseada - objetivoSara.position;
        if (separacion.magnitude < distanciaMinimaSeguridad)
        {
            separacion = separacion.normalized * distanciaMinimaSeguridad;
            posicionDeseada = objetivoSara.position + separacion;
        }

        transform.position = Vector3.SmoothDamp(transform.position, posicionDeseada, ref velocidadSmoothDamp, 1f / suavidadMovimiento);
    }

    // Evita que el jefe salga de los límites configurados del Tilemap
    void AplicarLimitesEscenario()
    {
        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        float clampedY = Mathf.Clamp(transform.position.y, minY, maxY);
        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }

    // Efecto visual sutil de contracción y expansión (simulando distancia/latido)
    IEnumerator RutinaLatidoEscala()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(4f, 7f));

            if (!estaAtacando)
            {
                float duracion = 1.2f;
                float tiempo = 0f;
                Vector3 escalaLejana = escalaOriginal * 0.8f; // Reducción sutil y natural

                while (tiempo < duracion)
                {
                    transform.localScale = Vector3.Lerp(transform.localScale, escalaLejana, tiempo / duracion);
                    tiempo += Time.deltaTime;
                    yield return null;
                }

                yield return new WaitForSeconds(2f);

                tiempo = 0f;
                while (tiempo < duracion)
                {
                    transform.localScale = Vector3.Lerp(transform.localScale, escalaOriginal, tiempo / duracion);
                    tiempo += Time.deltaTime;
                    yield return null;
                }
            }
        }
    }

    // Cooldowns y selección de ataques
    IEnumerator BuclePrincipalIA()
    {
        while (true)
        {
            // Cooldown de descanso entre ataques
            float tiempoCooldown = Random.Range(minCooldown, maxCooldown);
            yield return new WaitForSeconds(tiempoCooldown);

            if (objetivoSara != null && !estaAtacando)
            {
                int seleccionAtaque = Random.Range(0, 2);

                if (seleccionAtaque == 0)
                {
                    yield return StartCoroutine(AtaquePasadaHorizontal());
                }
                else
                {
                    yield return StartCoroutine(AtaqueAmagueCercano());
                }
            }
        }
    }

    // Ataque 1: Cruce horizontal a la altura de Sara
    IEnumerator AtaquePasadaHorizontal()
    {
        estaAtacando = true;
        transform.localScale = escalaOriginal;

        if (spriteRenderer != null) spriteRenderer.color = Color.yellow;
        yield return new WaitForSeconds(0.7f);

        Vector3 posSara = objetivoSara.position;
        float direccionLado = (Random.value > 0.5f) ? 1f : -1f;

        Vector3 puntoInicio = new Vector3(Mathf.Clamp(posSara.x + (70f * direccionLado), minX, maxX), posSara.y, 0f);
        Vector3 puntoFinal = new Vector3(Mathf.Clamp(posSara.x + (-70f * direccionLado), minX, maxX), posSara.y, 0f);

        transform.position = puntoInicio;
        if (spriteRenderer != null) spriteRenderer.color = colorOriginal;

        float crono = 0f;
        float duracionCruze = 1.6f;
        while (crono < duracionCruze)
        {
            transform.position = Vector3.Lerp(puntoInicio, puntoFinal, crono / duracionCruze);
            crono += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.6f);
        velocidadSmoothDamp = Vector3.zero;
        estaAtacando = false;
    }

    // Ataque 2: Amague rápido y cercano
    IEnumerator AtaqueAmagueCercano()
    {
        estaAtacando = true;
        transform.localScale = escalaOriginal;

        Vector3 posAviso = transform.position;
        if (spriteRenderer != null) spriteRenderer.color = Color.red;

        float t = 0f;
        while (t < tiempoAviso)
        {
            transform.position = posAviso + (Vector3)Random.insideUnitCircle * 0.3f;
            t += Time.deltaTime;
            yield return null;
        }

        Vector3 posSara = objetivoSara.position;
        Vector3 direccion = (posSara - transform.position).normalized;
        Vector3 puntoCercano = posSara - (direccion * 2f);

        if (spriteRenderer != null) spriteRenderer.color = colorOriginal;

        float tAtaque = 0f;
        while (tAtaque < 0.15f)
        {
            transform.position = Vector3.MoveTowards(transform.position, puntoCercano, velocidadAtaque * Time.deltaTime);
            tAtaque += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.4f);
        yield return new WaitForSeconds(0.7f);
        velocidadSmoothDamp = Vector3.zero;
        estaAtacando = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && estaAtacando)
        {
            Debug.Log("Sara 1 recibió daño.");
        }
    }
}