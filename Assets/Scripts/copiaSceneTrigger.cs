using System;
using UnityEngine;
using UnityEngine.Playables;

public class copiaSceneTrigger : MonoBehaviour
{
    [SerializeField] private PlayableDirector director;
    [SerializeField] private GameObject cutsceneCanvas;

    public event Action CutsceneFinished;   // <-- new

    private bool played;
    private PlayerController player;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out PlayerController p))
            StartCutscene(p);
    }

    // Public so other scripts can start the cutscene
    public void StartCutscene(PlayerController p)
    {
        if (played) return;
        played = true;
        player = p;

        if (TryGetComponent(out Collider2D col)) col.enabled = false;
        foreach (var sr in GetComponentsInChildren<SpriteRenderer>())
            sr.enabled = false;

        if (player.TryGetComponent(out Rigidbody2D rb))
            rb.linearVelocity = Vector2.zero;
        player.enabled = false;

        cutsceneCanvas.SetActive(true);
        director.stopped += OnCutsceneEnd;
        director.Play();
    }

    private void OnCutsceneEnd(PlayableDirector d)
    {
        d.stopped -= OnCutsceneEnd;

        cutsceneCanvas.SetActive(false);
        if (player != null) player.enabled = true;

        CutsceneFinished?.Invoke();   // <-- fire BEFORE destroying
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (director != null) director.stopped -= OnCutsceneEnd;
    }
}
