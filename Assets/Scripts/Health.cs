using UnityEngine;

public class Health : MonoBehaviour
{
    public int currentHealth = 50;
    public bool dead = false;
    public GameObject trampa;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (dead == true)
        {
            Destroy(trampa);
        }
    }
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            gameObject.SetActive(false);
            dead = true;
            if (dead == true)
            {
                Destroy(trampa);
            }
        }
            
    }
}
