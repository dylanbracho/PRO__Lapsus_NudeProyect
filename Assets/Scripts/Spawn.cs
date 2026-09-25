using System.Collections;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField] GameObject coagulito;
    [SerializeField] GameObject spawn;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SpawnOHSI());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnOHSI() 
    {
        int ustednosientequetodoserepite = 1;

        

        while (ustednosientequetodoserepite < 50) 
        {
            yield return new WaitForSeconds(6f);
            Instantiate(coagulito, spawn.transform.position, spawn.transform.rotation);
            ustednosientequetodoserepite++;
        }

        
       
    } 
}
