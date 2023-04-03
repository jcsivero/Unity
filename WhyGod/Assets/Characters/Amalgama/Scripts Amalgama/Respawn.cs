using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField] public Vector2 RespawnPoint;
    // Start is called before the first frame update
    void Start()
    {
        RespawnPoint = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Respawn")
        {
            Debug.Log("RESPAWN RESETEADO");
            RespawnPoint = collision.collider.transform.position;
        }
    }
}