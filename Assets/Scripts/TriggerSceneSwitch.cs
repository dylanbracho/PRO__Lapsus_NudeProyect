using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class SceneTrigger2D : MonoBehaviour
{
    [SerializeField] private string sceneToLoad = "Level2";
    [SerializeField] private bool sceneSwitch = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (sceneSwitch == true && other.CompareTag("Player"))
        {
            StartCoroutine(SceneSwitcher());
            
        } 
        else if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    IEnumerator SceneSwitcher()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(sceneToLoad);
    }
}