using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private GameObject mover;
    [SerializeField] private GameObject dash;
    [SerializeField] private GameObject jump;
    [SerializeField] private GameObject attack;
    [SerializeField] private GameObject coagulito;

    private bool pressedA;
    private bool pressedD;

    void Update()
    {
        var kb = Keyboard.current;
        if (kb == null) return;

        if (kb.aKey.wasPressedThisFrame) pressedA = true;
        if (kb.dKey.wasPressedThisFrame) pressedD = true;

        if (pressedA && pressedD)
        {
            StartCoroutine(Mover());
        }

        if (kb.spaceKey.wasPressedThisFrame)
        {
            StartCoroutine(Jump());
        }

        if (kb.leftShiftKey.wasPressedThisFrame)
        {
            StartCoroutine(Dash());
        }

        if (!coagulito.activeSelf)
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Mover()
    {
        yield return new WaitForSeconds(3f);
        mover.SetActive(false);
    }

    IEnumerator Dash()
    {
        yield return new WaitForSeconds(3f);
        dash.SetActive(false);
    }

    IEnumerator Jump()
    {
        yield return new WaitForSeconds(3f);
        jump.SetActive(false);
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(3f);
        attack.SetActive(false);
    }
}