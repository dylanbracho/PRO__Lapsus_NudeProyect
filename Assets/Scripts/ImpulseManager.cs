using Unity.Cinemachine;
using UnityEngine;

public class ImpulseManager : MonoBehaviour
{
    public static ImpulseManager instance;
    [SerializeField] private float impulseForce = 1f;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void CameraShake(CinemachineImpulseSource impulseSource)
    {
        impulseSource.GenerateImpulseWithForce(impulseForce);
    }
}
