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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        

        health = maxhealth;
        healthBarSlider.value = health;
        healthBarSlider.maxValue = maxhealth;
        vidaentexto.text = $"HP: {health}/{maxhealth}";
        imagennucleo.color = new Color(1, 1, 1, 0);
        collidernucleo.enabled = false;
        StartCoroutine(NucleoOHSI_());
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {

            SceneManager.LoadScene(sceneToLoad);

        }
    }

    IEnumerator NucleoOHSI_() 
    {

        while (true)
        {
            yield return new WaitForSeconds(7f);
            imagennucleo.color = new Color(1, 1, 1, 1);
            collidernucleo.enabled = true;
            yield return new WaitForSeconds(3f);
            imagennucleo.color = new Color(1, 1, 1, 0);
            collidernucleo.enabled = false;
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

    
}
