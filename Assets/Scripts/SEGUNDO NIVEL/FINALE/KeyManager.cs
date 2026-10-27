using UnityEngine;

public class KeyManager : MonoBehaviour
{
    public static KeyManager Instance;

    public int totalPiecesNeeded = 3;
    public int currentPieces = 0;
    public bool hasFullKey = false;

    void Awake()
    {
        // Keeps a single active instance
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CollectPiece()
    {
        currentPieces++;
        Debug.Log("Key Piece Collected: " + currentPieces + "/" + totalPiecesNeeded);

        if (currentPieces >= totalPiecesNeeded)
        {
            hasFullKey = true;
            Debug.Log("Heart Locket Key Assembled!");
        }
    }
}