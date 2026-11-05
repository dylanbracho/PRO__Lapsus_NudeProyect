using UnityEngine;

public class PlayBlood : MonoBehaviour
{
    private PlayerController pc;
    public void PlaySoundBlood()
    {
        
            SFXManager.PlaySFX(Soundtype.BLOOD, 0.03f);
        
    }
}
