using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class nucleo : MonoBehaviour
{
    [SerializeField] GameObject nucleoOHSI;
    [SerializeField] Collider2D collidernucleo;
    [SerializeField] SpriteRenderer imagennucleo;
    [SerializeField] bool isActive;

    public float health;
    public float maxhealth;

    public Slider healthBarSlider;
    public TextMeshProUGUI vidaentexto;

    [SerializeField] private string sceneToLoad;
    private bool isDead = false; // Evita que se llame a LoadScene varias veces seguidas

    void Start()
    {
        health = maxhealth;

        if (healthBarSlider != null)
        {
            healthBarSlider.maxValue = maxhealth;
            healthBarSlider.value = health;
        }

        if (vidaentexto != null)
        {
            vidaentexto.text = $"HP: {health}/{maxhealth}";
        }

        if (imagennucleo != null)
        {
            imagennucleo.color = new Color(1, 1, 1, 0);
        }

        if (collidernucleo != null)
        {
            collidernucleo.enabled = false;
        }

        StartCoroutine(NucleoOHSI_());
    }

    void Update()
    {
        // Si la vida llega a 0 y no hemos cambiado de escena aún, cambia a la escena deseada
        if (health <= 0 && !isDead)
        {
            CargarSiguienteEscena();
        }
    }

    IEnumerator NucleoOHSI_()
    {
        while (true)
        {
            yield return new WaitForSeconds(7f);
            if (imagennucleo != null) imagennucleo.color = new Color(1, 1, 1, 1);
            if (collidernucleo != null) collidernucleo.enabled = true;

            yield return new WaitForSeconds(3f);
            if (imagennucleo != null) imagennucleo.color = new Color(1, 1, 1, 0);
            if (collidernucleo != null) collidernucleo.enabled = false;
        }
    }

    public void RecibirDaño(float cantidad)
    {
        if (isDead) return;

        health -= cantidad;
        health = Mathf.Clamp(health, 0, maxhealth);

        ActualizarInterfaz();

        // Al recibir daño y quedarse sin vida, cambia de escena inmediatamente
        if (health <= 0)
        {
            CargarSiguienteEscena();
        }
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

    void CargarSiguienteEscena()
    {
        isDead = true;
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("No colocaste el nombre de la escena en 'sceneToLoad' en el Inspector.");
        }
    }
}