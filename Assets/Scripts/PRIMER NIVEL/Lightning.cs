using System.Collections;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    [SerializeField] private GameObject[] lightningPrefab;
    bool isLightningActive = true;
    // Update is called once per frame
    void Start()
    {
        StartCoroutine(LightningStrike());
    }

    IEnumerator LightningStrike()
    {
        while (isLightningActive)
        {
            for (int i = 0; i < lightningPrefab.Length; i++)
            {
                yield return new WaitForSeconds(5f);
                lightningPrefab[i].SetActive(true);
                SFXManager.PlaySFX(Soundtype.THUNDER, 0.6f);
                yield return new WaitForSeconds(0.5f);
                lightningPrefab[i].SetActive(false);
            }
        }
    }
}
