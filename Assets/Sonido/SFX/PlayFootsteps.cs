using UnityEngine;

public class PlayFootsteps : MonoBehaviour
{
    private PlayerController pc;
    public void Awake()
    {
        pc = FindAnyObjectByType<PlayerController>();
    }
    public void PlaySoundFootsteps()
    {
        if (pc.canJump)
        {
            SFXManager.PlaySFX(Soundtype.STEP, 0.15f);
        }
    }
}
