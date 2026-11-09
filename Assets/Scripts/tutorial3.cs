using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tutorial3 : MonoBehaviour
{
    [SerializeField] private GameObject attack;
    [SerializeField] private GameObject coagulito;

    void Update()
    {
        var kb = Keyboard.current;

        if (kb.qKey.wasPressedThisFrame)
        {
            StartCoroutine(Mazo(3f));
        }
    }

    IEnumerator Mazo(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        attack.SetActive(false);
    }
}