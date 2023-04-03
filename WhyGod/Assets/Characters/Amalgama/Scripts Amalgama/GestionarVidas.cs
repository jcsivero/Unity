using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestionarVidas : MonoBehaviour
{
   
    [SerializeField] public int MaxVidasAmalgama = 3;
    private int VidasAmalgama;

    private Respawn respawn;

    private void Start()
    {
        respawn = FindAnyObjectByType<Respawn>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(VidasAmalgama);
        if (VidasAmalgama <= 0)
        {
            Morir();
        }
    }

    public void RecibirDa�o(int da�o)
    {
        // ANIMACION DE RECIBIR GOLPE
        VidasAmalgama -= da�o;
    }

    public void Morir()
    {
        // ANIMACION DE MORIR
        VidasAmalgama = MaxVidasAmalgama;
        transform.position = respawn.RespawnPoint;
    }
}
