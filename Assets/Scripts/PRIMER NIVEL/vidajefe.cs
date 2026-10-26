using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class vidajefe : MonoBehaviour
{

    public float health;
    public float maxhealth;

    public Slider healthBarSlider;
    public TextMeshProUGUI vidaentexto;

    [SerializeField] private string sceneToLoad;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxhealth;
        healthBarSlider.value = health;
        healthBarSlider.maxValue = maxhealth;
        vidaentexto.text = $"HP: {health}/{maxhealth}";
       

    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0)
        {
            
            SceneManager.LoadScene(sceneToLoad);
            
        }
    }

    public void RecibirDaño(float cantidad)
    {
        health -= cantidad;
        health = Mathf.Clamp(health, 0, maxhealth);

        ActualizarInterfaz();

        Debug.Log("aaaa");
    }

    void ActualizarInterfaz()
    {
        if (healthBarSlider != null)
        {
            healthBarSlider.maxValue = maxhealth;
            healthBarSlider.value = health;
        }

        if (vidaentexto != null)
        {
            vidaentexto.text = $"HP: {health}/{maxhealth}";
        }
    }

    public void TakeDamage(float amount)
    {
        RecibirDaño(amount);
        Debug.Log("¡Le pegaste a la flor rosa del jefe!");
    }
}




