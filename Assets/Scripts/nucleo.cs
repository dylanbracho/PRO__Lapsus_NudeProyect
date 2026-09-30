using UnityEngine;
using System.Collections;


public class nucleo : MonoBehaviour
{
    [SerializeField] GameObject nucleoOHSI;
    [SerializeField] bool isActive;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine("NucleoOHSI_");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator NucleoOHSI_() 
    {
       

        while (true)
        {
            yield return new WaitForSeconds(2f);
            nucleoOHSI.SetActive(isActive);
            isActive = !isActive;
            //stednosientequetodoserepite++;
        }
        
    }
}
