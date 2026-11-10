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

    [Header("Cambio de Escena al Destruirse")]
    [SerializeField] private string sceneToLoad;
    private bool hasChangedScene = false;

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
        // Control de seguridad por si la salud llega a 0 por otro medio
        if (health <= 0 && !hasChangedScene)
        {
            CambiarEscena();
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
        if (hasChangedScene) return;

        health -= cantidad;
        health = Mathf.Clamp(health, 0, maxhealth);

        ActualizarInterfaz();

        // Cambia de escena de inmediato si el golpe deja el health en 0
        if (health <= 0)
        {
            CambiarEscena();
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

    void CambiarEscena()
    {
        hasChangedScene = true;
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("No has escrito el nombre de la escena en la variable 'Scene To Load' del Núcleo.");
        }
    }
}