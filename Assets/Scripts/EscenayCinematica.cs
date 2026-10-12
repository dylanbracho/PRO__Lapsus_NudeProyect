using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class EscenayCinematica : MonoBehaviour
{
    [SerializeField] private copiaSceneTrigger cutsceneTrigger;
    [SerializeField] private string sceneToLoad = "Level2";
    private bool started;
    private void OnEnable() => cutsceneTrigger.CutsceneFinished += GoToNextScene;
    private void OnDisable() => cutsceneTrigger.CutsceneFinished -= GoToNextScene;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (started) return;
        if (!other.TryGetComponent(out PlayerController player)) return;

        started = true;
        cutsceneTrigger.StartCutscene(player);
    }

    private void GoToNextScene()
    {
        SwitchScene();   // use whatever your method is actually called
    }

    public void SwitchScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

}
