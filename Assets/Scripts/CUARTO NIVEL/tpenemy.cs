using System;
using UnityEngine;
using System.Collections;

public class tpenemy : MonoBehaviour // O herencia de Teleport si lo requiere tu proyecto
{
    [SerializeField] GameObject[] sppoints;


    void Start()
    {
        StartCoroutine(TeleportOHSI());
    }

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

            // Valida que existan puntos asignados antes de teletransportar
            if (sppoints.Length > 0 && sppoints[order] != null)
            {
                transform.position = sppoints[order].transform.position;
            }

            order++;

            // Si llega al último punto, regresa al primero (índice 0) de forma segura
            if (order >= sppoints.Length)
            {
                order = 0;
            }

            ustednosientequetodoserepite++;
        }
    }
}