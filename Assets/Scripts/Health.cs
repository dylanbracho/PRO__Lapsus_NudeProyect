using UnityEngine;

public class Health : MonoBehaviour
{
    public int currentHealth = 50;
    public bool dead = false;
    [SerializeField] private bool espejo = false;
    [SerializeField] private bool sucubo = false;
    public GameObject trampa;
    public GameObject objeto;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (dead == true && espejo == false)
        {
            Destroy(trampa);
        }
        if (dead == true && sucubo == true)
        {
            objeto.SetActive(true);
        }
    }
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            gameObject.SetActive(false);
            dead = true;
            if (dead == true && espejo == false)
            {
                Destroy(trampa);
            }
            if (dead == true && sucubo == true)
            {
                objeto.SetActive(true);
            }
        }
            
    }
}
