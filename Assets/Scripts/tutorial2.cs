using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tutorial2 : MonoBehaviour
{
    [SerializeField] private GameObject attack;
    [SerializeField] private GameObject coagulito;

    void Update()
    {
        var kb = Keyboard.current;

        if (kb.rKey.wasPressedThisFrame)
        {
            StartCoroutine(WaitForSeconds(3f));
            //attack.SetActive(false);
        }
    }

    IEnumerator WaitForSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        attack.SetActive(false);
    }
}