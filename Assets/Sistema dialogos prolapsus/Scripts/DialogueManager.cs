using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public Queue<string> sentences;

    public TextMeshProUGUI npcNameText;
    public TextMeshProUGUI npcDialogueText;

    public Animator animator;

    [Range(0.05f, 0f)]
    public float textAnimationSpeed;

    string sentenceInProgress;
    [SerializeField] private AudioClip[] sounds;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private bool stopAudio;
    [Range  (0f, 0.5f)]
    [SerializeField] private float minPitch = 0.5f;
    [Range  (0f, 2f)]
    [SerializeField] private float maxPitch = 3f;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        audioSource = GetComponent<AudioSource>();
    }

    public void StartDialogue(Dialogue _dialogue)
    {
        animator.SetBool("IsOpen", true);

        npcNameText.text = _dialogue.npcName;

        sentences.Clear();

        foreach (string sentence in _dialogue.npcSentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {

        if (sentenceInProgress != null && npcDialogueText.text != sentenceInProgress) // La frase no ha terminado de animarse
        {
            npcDialogueText.text = sentenceInProgress;
            StopAllCoroutines();
            return;
        }

        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        StopAllCoroutines();
        string sentence = sentences.Dequeue();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        sentenceInProgress = sentence;

        npcDialogueText.text = "";
        int charIndex = 0;
        foreach (char letter in sentence.ToCharArray())
        {
            if (!char.IsWhiteSpace(letter))
            {
                PlaySound(charIndex);
            }
            npcDialogueText.text += letter;
            charIndex++;
            yield return new WaitForSeconds(textAnimationSpeed);
        }
    }

    void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
    }

    private void PlaySound(int displayCharacters)
    {
        if (displayCharacters % 2 == 0)
        {
            if (stopAudio)
            {
                audioSource.Stop();
            }
            int randomIndex = Random.Range(0, sounds.Length);
            AudioClip soundClip = sounds[randomIndex];
            audioSource.pitch = Random.Range(minPitch, maxPitch);
            audioSource.PlayOneShot(soundClip);
        }
    }

}
