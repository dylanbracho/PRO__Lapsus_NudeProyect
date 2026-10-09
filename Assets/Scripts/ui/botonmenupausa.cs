using UnityEngine;
using UnityEngine.SceneManagement;

public class botonmenupausa : MonoBehaviour
{

    public GameObject menupausa;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Despausa()
    {
        menupausa.SetActive(false);
        Time.timeScale = 1f;
    }

    [SerializeField] private string sceneToLoad;


    public void Menuprincipal()
    {

        SceneManager.LoadScene(sceneToLoad); 


    }
}
