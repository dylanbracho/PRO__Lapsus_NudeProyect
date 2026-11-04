using UnityEngine;

public class PlayBlood : MonoBehaviour
{
    private PlayerController pc;
    public void Awake()
    {
        pc = FindAnyObjectByType<PlayerController>();
    }
    public void PlaySoundBlood()
    {
        if (pc.canJump)
        {
            SFXManager.PlaySFX(Soundtype.BLOOD, 0.05f);
        }
    }
}
