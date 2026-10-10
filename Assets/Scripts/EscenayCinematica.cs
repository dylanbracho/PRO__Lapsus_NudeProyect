using UnityEngine;
using UnityEngine.Playables;

public class EscenayCinematica : MonoBehaviour
{
    [SerializeField] private copiaSceneTrigger cutsceneTrigger;
    [SerializeField] private SceneTrigger2D sceneTrigger;
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
        sceneTrigger.SwitchScene();   // use whatever your method is actually called
    }

}
