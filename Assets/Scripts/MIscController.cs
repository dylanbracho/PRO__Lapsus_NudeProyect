using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTrigger : MonoBehaviour
{
    [SerializeField] private PlayableDirector director;
    [SerializeField] private GameObject cutsceneCanvas;
    [SerializeField] private GameObject bloqueo;
    [SerializeField] private GameObject trampa;


    public bool played;
    public PlayerController player;

     public void OnTriggerEnter2D(Collider2D other)
    {
        if (played) return;
        if (!other.TryGetComponent(out player)) return; // only react to the player

        played = true;

        // hide the collectable but keep this object alive until the cutscene ends
        GetComponent<Collider2D>().enabled = false;
        foreach (var sr in GetComponentsInChildren<SpriteRenderer>())
            sr.enabled = false;

        // stop the player from moving
        if (other.TryGetComponent(out Rigidbody2D rb))
            rb.linearVelocity = Vector2.zero; // use rb.velocity on older Unity versions
        player.enabled = false;

        cutsceneCanvas.SetActive(true);
        director.stopped += OnCutsceneEnd;
        director.Play();
    }

    void OnCutsceneEnd(PlayableDirector d)
    {
        d.stopped -= OnCutsceneEnd;
        cutsceneCanvas.SetActive(false);
        player.enabled = true;
        bloqueo.SetActive(false);
        trampa.SetActive(true);
        Destroy(gameObject); // collectable is gone for good
    }

    
}