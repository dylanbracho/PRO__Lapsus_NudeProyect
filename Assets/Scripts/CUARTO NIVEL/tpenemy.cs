using System;
using UnityEngine;
using System.Collections;
//using Unity.Mathematics;

public class tpenemy : Teleport
{
    [SerializeField] GameObject[] sppoints;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //base.TeleportCharacter();
        StartCoroutine(TeleportOHSI());
    }

    // Update is called once per frame
    void Update()
    {

        
    }

    IEnumerator TeleportOHSI()
    {
        int ustednosientequetodoserepite = 0;
        int order = 0;



        while (ustednosientequetodoserepite < 10)
        {
            yield return new WaitForSeconds(6f);
            transform.position = sppoints[order].transform.position;
            order++;
            if (order>sppoints.Length)
            {
                order = 0;
            }
            ustednosientequetodoserepite++;
        }



    }

}
