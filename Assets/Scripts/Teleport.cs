using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject tp;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TeleportCharacter()
    {
        player.transform.position = tp.transform.position;
    }
}
