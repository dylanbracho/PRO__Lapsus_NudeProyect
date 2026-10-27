using UnityEngine;

public class KeyPiece : MonoBehaviour
{
    private bool collected = false; // Seguro para que solo se cuente una vez

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si ya fue recolectado, no hace nada
        if (collected) return;

        // Si lo toca el jugador
        if (collision.GetComponent<PlayerController>() != null || collision.name.Contains("Sara"))
        {
            collected = true; // Bloquea para que no vuelva a entrar

            if (KeyManager.Instance != null)
            {
                KeyManager.Instance.CollectPiece();
            }

            Destroy(gameObject); // Destruye la pieza al instante
        }
    }
}