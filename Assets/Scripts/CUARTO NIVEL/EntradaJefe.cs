using UnityEngine;

public class EntradaJefe : MonoBehaviour
{
    [Header("Referencia al Jefe")]
    [SerializeField] private GameObject jefe;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Sara cruzó el Trigger!");

            if (jefe != null)
            {
                BasicMov scriptMov = jefe.GetComponent<BasicMov>();
                if (scriptMov != null)
                {
                    scriptMov.ActivarJefe();
                }
                else
                {
                    Debug.LogError("No se encontró el script BasicMov en el Jefe!");
                }
            }

            gameObject.SetActive(false);
        }
    }
}