using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlturaMuerte : MonoBehaviour
{
    private GestionarVidas amalgama;

    private void Start()
    {
        amalgama = FindAnyObjectByType<GestionarVidas>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        amalgama.Morir();
    }
}
