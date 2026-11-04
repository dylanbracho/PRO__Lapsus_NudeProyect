using UnityEngine;
using System;
public enum Soundtype
{
    STEP,
    WHIP,
    JUMP,
    LAND,
    HURT,
    MAGIC,
    BLOOD,
    THUNDER
}

[RequireComponent(typeof(AudioSource)) , ExecuteInEditMode]
public class SFXManager : MonoBehaviour
{
    [SerializeField] private SoundList[] soundList;
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
        AudioClip[] clips = instance.soundList[(int)sound].Sounds;
        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];
        instance.audioSource.PlayOneShot(randomClip, volume);
    }
#if UNITY_EDITOR
    private void OnEnable()
    {
        string[] names = Enum.GetNames(typeof(Soundtype));
        Array.Resize(ref soundList, names.Length);
        for (int i = 0; i < soundList.Length; i++)
        {
            soundList[i].name = names[i];
        }
    }
#endif
}

[Serializable]
public struct SoundList
{
    public AudioClip[] Sounds { get => sounds;}
    [HideInInspector] public string name;
    [SerializeField] private AudioClip[] sounds;
}
