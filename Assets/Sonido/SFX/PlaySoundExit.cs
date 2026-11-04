using UnityEngine;

public class PlaySoundExit : StateMachineBehaviour
{
    [SerializeField] private Soundtype sound;
    [SerializeField, Range(0, 1)] private float volume = 1;
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SFXManager.PlaySFX(sound, volume);
    }
}
