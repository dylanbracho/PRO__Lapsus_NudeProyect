using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class vidajefe4 : MonoBehaviour
{
    public float health;
    public float maxhealth = 100f;

    public Slider healthBarSlider;
    public TextMeshProUGUI vidaentexto;

    [SerializeField] private string sceneToLoad;

    void Start()
    {
        health = maxhealth;

        if (healthBarSlider != null)
        {
            healthBarSlider.value = health;
            healthBarSlider.maxValue = maxhealth;
        }

        if (vidaentexto != null)
        {
            vidaentexto.text = $"HP: {health}/{maxhealth}";
        }
    }

    void Update()
    {
        if (health <= 0)
        {
            if (!string.IsNullOrEmpty(sceneToLoad))
            {
                SceneManager.LoadScene(sceneToLoad);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void RecibirDaño(float cantidad)
    {
        health -= cantidad;
        health = Mathf.Clamp(health, 0, maxhealth);

        ActualizarInterfaz();

        Debug.Log("¡Daño recibido por el Heart Enemy!");
    }

    // Este es el método Curar que faltaba en vidajefe4
    public void Curar(float cantidad)
    {
        health += cantidad;
        health = Mathf.Clamp(health, 0, maxhealth);

        ActualizarInterfaz();
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
}