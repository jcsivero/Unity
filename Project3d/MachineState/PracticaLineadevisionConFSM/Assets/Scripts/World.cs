using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public sealed class World :MonoBehaviour
{
    private static readonly World instance = new World();
    private static GameObject[] hidingSpots;
    public int valor =8;

    static World()
    {
        //hidingSpots = GameObject.FindGameObjectsWithTag("hide");
        
    }
    ~World()  
    {
        Debug.Log("Destruida instancia World desde destructor");
        Console.WriteLine("Destruida instancia World desde destructor Writeline");
    } 
    private World() { 
        Debug.Log("Iniciado instancia World");
        Console.WriteLine("Constructor  World desde destructor Writeline"); 

        /// <summary>
        /// This function is called when the MonoBehaviour will be destroyed.
        /// </summary>

    }
        void OnDestroy()
        {
                Debug.Log("Destruida instancia World desde ondestroy");
                Console.WriteLine("Destruida instancia World desde ondestroy Writeline");
        }

    public static World Instance
    {
        get { return instance; }
    }

    public GameObject[] GetHidingSpots()
    {
        return hidingSpots;
    }
}
