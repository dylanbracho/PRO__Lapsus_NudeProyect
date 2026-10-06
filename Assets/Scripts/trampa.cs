using UnityEngine;
using UnityEngine.SceneManagement;

public class trampa : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject trampaTrigger;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            trampaTrigger.SetActive(true);
        }
        
    }
}
