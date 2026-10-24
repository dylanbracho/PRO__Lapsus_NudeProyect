using UnityEngine;

public class copiaTeleport : MonoBehaviour
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        TeleportCharacter();
    }

    protected void TeleportCharacter()
    {
        player.transform.position = tp.transform.position;
    }
}
