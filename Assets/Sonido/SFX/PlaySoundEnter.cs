using UnityEngine;

public class PlaySoundEnter : StateMachineBehaviour
{
    [SerializeField] private Soundtype sound;
    [SerializeField, Range(0,1)] private float volume = 1;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SFXManager.PlaySFX(sound, volume);
    }

   
}
