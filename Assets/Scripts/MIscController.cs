using UnityEngine;

public class MIscController : MonoBehaviour
{

    public GameObject objeto;
    public Collider2D collider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            collider.isTrigger = true;
            objeto.SetActive(false);
            
        }


    }
}
