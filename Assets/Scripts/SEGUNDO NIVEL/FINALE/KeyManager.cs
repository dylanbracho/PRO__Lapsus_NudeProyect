using UnityEngine;

public class KeyManager : MonoBehaviour
{
    // Patrón Singleton para que sea único y accesible desde cualquier escena
    public static KeyManager Instance { get; private set; }

    
    public int currentPieces = 0;
    public int totalPiecesNeeded = 3;
    public bool hasFullKey = false;

    private void Awake()
    {
        // Configuración del Singleton y DontDestroyOnLoad
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Esto evita que se destruya al cambiar de escena
        }
        else
        {
            Destroy(gameObject); // Evita duplicados si regresas a la escena anterior
            return;
        }
    }

    public void CollectPiece()
    {
        currentPieces++;
        Debug.Log("Pieza de llave recolectada. Total: " + currentPieces + "/" + totalPiecesNeeded);

        if (currentPieces >= totalPiecesNeeded)
        {
            hasFullKey = true;
            Debug.Log("¡Llave completa! El Heart Locket está listo.");
        }
    }
}