using UnityEngine;

public class AtaquesBoss : MonoBehaviour
{
    [SerializeField] private ParticleSystem sistemaSangre;

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
                LanzarEstampida();
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
            Vector2 direccion = (posicionSara - sistemaSangre.transform.position).normalized;

           
            float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;

            
            sistemaSangre.transform.rotation = Quaternion.Euler(0f, 0f, angulo - 90f);

            sistemaSangre.Play();
        }
    }

    private void LanzarEstampida()
    {

    }

    private void LanzarTercerAtaque()
    {

    }
}