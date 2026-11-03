using UnityEngine;
public enum Soundtype
{
    STEP,
    WHIP,
    JUMP,
    LAND,
    HURT,
    MAGIC,
    BLOOD
}

[RequireComponent(typeof(AudioSource))]
public class SFXManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] sounds;
    private static SFXManager instance;
    private AudioSource audioSource;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySFX(Soundtype sound, float volume = 1f)
    {
        instance.audioSource.PlayOneShot(instance.sounds[(int)sound], volume);
    }
}
