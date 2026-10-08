using UnityEngine;

public class UIManager : MonoBehaviour
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

    public void Activarmenu()
    {
        if (menupausa.activeInHierarchy)
        {
            menupausa.SetActive(false);
            Time.timeScale = 1f;
        }
        else if(!menupausa.activeInHierarchy)
        {
            menupausa.SetActive(true);
            Time.timeScale = 0f;

        }
    }

}
