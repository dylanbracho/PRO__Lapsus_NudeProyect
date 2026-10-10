using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class SceneTrigger2D : MonoBehaviour
{
    [SerializeField] private string sceneToLoad = "Level2";

    public void SwitchScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}