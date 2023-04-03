using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlataforma : MonoBehaviour
{
    public GameObject[] puntos;

    public float velocidadPlataforma;

    public int index;

    // Update is called once per frame
    public void Update()
    {
        MoverPlataforma();
    }

    private void MoverPlataforma()
    {
        if (Vector2.Distance(transform.position, puntos[index].transform.position) < 0.1f)
        {
            index = (index + 1) % puntos.Length;
        }
        transform.position = Vector2.MoveTowards(transform.position, 
                                                 puntos[index].transform.position, 
                                                 velocidadPlataforma * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.SetParent(null);
        }
    }
}
