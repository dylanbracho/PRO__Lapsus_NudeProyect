using UnityEngine;
using System.Collections;

public class Espinas : MonoBehaviour
{
    [SerializeField] float altura = 1f;
    [SerializeField] float velocidad = 8f;
    [SerializeField] float tiempoEscondidas = 2f;
    [SerializeField] float tiempoAdvertencia = 1f;
    [SerializeField] float tiempoAturdido = 0.2f;
    [SerializeField] float tiempoArriba = 1f;
    [SerializeField] float daño = 1f;

    Vector3 abajo, arriba;
    bool peligrosa, yaGolpeo;

    void Start()
    {
        abajo = transform.position;
        arriba = abajo + Vector3.up * altura;
        StartCoroutine(Ciclo());
    }

    IEnumerator Ciclo()
    {
        while (true)
        {
            yield return new WaitForSeconds(tiempoEscondidas);

            float t = 0f;
            while (t < tiempoAdvertencia)
            {
                transform.position = abajo + Vector3.right * Random.Range(-0.05f, 0.05f);
                t += Time.deltaTime;
                yield return null;
            }
            transform.position = abajo;

            peligrosa = true;
            yaGolpeo = false;
            yield return Mover(arriba);
            yield return new WaitForSeconds(tiempoArriba);
            yield return Mover(abajo);
            peligrosa = false;
        }
    }

    IEnumerator Mover(Vector3 destino)
    {
        while (transform.position != destino)
        {
            transform.position = Vector3.MoveTowards(transform.position, destino, velocidad * Time.deltaTime);
            yield return null;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!peligrosa || yaGolpeo) return;

        PlayerController jugador = other.GetComponentInParent<PlayerController>();
        if (jugador != null)
        {
            PlayerController play = other.GetComponentInParent<PlayerController>();
            if (jugador != null && !jugador.isInvincible)
            {
                play.hitFromRight = transform.position.x > jugador.transform.position.x;
                play.hitTime = tiempoAturdido;

                play.TakeDamage(daño);
                yaGolpeo = true; 
            }
        }  
    }
}
