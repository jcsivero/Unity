using UnityEngine;
using System.Collections;
using UnityEngine.PS4;
using System;

public class TrophyUnlocker : MonoBehaviour
{
    public GameObject trophymanager;

    
    void Start()
    {
        Invoke("UnlockTrophy",15.0f);

    }

    public void UnlockTrophy()
    {
        Debug.Log("Desbloqueando Logro");
        trophymanager.GetComponent<TrophyManager>().UnlockTrophy(2);
        Debug.Log("Logro desbloqueado");


    }
    
}