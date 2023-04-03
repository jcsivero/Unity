using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpulsoPiedra : MonoBehaviour
{
    private Rigidbody2D rb2d;

    [SerializeField] public float fuerzaLanzamiento;

    [SerializeField] private int Da�o = 1;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") { collision.GetComponent<GestionarVidas>().RecibirDa�o(Da�o); }
        Destroy(gameObject);
    }

    private void Update()
    {
        rb2d.AddForce(transform.up * fuerzaLanzamiento);
    }   
}
