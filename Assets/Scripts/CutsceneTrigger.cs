using System;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class CutsceneTrigger : MonoBehaviour
{
    [SerializeField] private PlayableDirector director;
    [SerializeField] private GameObject cutsceneCanvas;
    [SerializeField] private GameObject bloqueo;
    public event Action CutsceneFinished;
    public bool played;
    public PlayerController player;

    [SerializeField] private GameObject coleccionableui;

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
            rb.linearVelocity = Vector2.zero; 
        player.enabled = false;

        cutsceneCanvas.SetActive(true);
        director.stopped += OnCutsceneEnd;
        director.stopped += Test;
        director.Play();

        
    }

    public void OnCutsceneEnd(PlayableDirector d)
    {
        d.stopped -= OnCutsceneEnd;
        cutsceneCanvas.SetActive(false);
        player.enabled = true;
        bloqueo.SetActive(false);
        // escena.SwitchScene();
        Destroy(gameObject);
        coleccionableui.SetActive(true);

    }

    
    public void Test(PlayableDirector d)
    {
        Debug.Log("JDEWFKWÑ");
    }


    
    
}