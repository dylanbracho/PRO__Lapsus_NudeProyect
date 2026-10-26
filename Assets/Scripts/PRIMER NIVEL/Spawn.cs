using System;
using System.Collections;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField] CaogulitoController coagulito;
    [SerializeField] GameObject spawn;
    [SerializeField] GameObject player;

    bool hasFightStarted;

    //[SerializeField] GameObject bloodPanel;
    //[SerializeField] GameObject[] espinas;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //StartCoroutine(SpawnOHSI());

        //StartCoroutine(SpawnEspinas());

    }

    // Update is called once per frame
    void Update()
    {
        if (true)
        {

        }
    }
    /*IEnumerator SpawnEspinas()
    {
        foreach (GameObject espina in espinas)
        {
            yield return new WaitForSeconds(1f);
            espina.SetActive(true);
        }
    }*/

    IEnumerator SpawnOHSI() 
    {
        int ustednosientequetodoserepite = 0;

        

        while (ustednosientequetodoserepite < 10) 
        {
            Debug.Log("Spawned Coagulito");
            yield return new WaitForSeconds(9f);
            CaogulitoController spawnedCoagulito = Instantiate(coagulito, spawn.transform.position, spawn.transform.rotation);
            spawnedCoagulito.enemyMovementPoints[0] = player.transform;
            ustednosientequetodoserepite++;
        }

        
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && !hasFightStarted)
        {
            hasFightStarted = true;
            //bloodPanel.SetActive(true);

            StartCoroutine(SpawnOHSI());
        }
    }
}
