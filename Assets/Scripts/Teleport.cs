using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] protected GameObject player;
    [SerializeField] protected GameObject tp;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   
    protected void TeleportCharacter()
    {
        player.transform.position = tp.transform.position;
    }
}
