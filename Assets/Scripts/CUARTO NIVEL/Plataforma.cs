using UnityEngine;

public class Plataforma : MonoBehaviour
{
    [SerializeField]float metaP;
    [SerializeField]float speedP;

    bool activateP;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y<metaP && activateP)
        {
            transform.position += new Vector3(0f, speedP) * Time.deltaTime;

        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Simon lo mejor");
        activateP = true;
    }
}
