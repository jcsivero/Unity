using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraSmooth : MonoBehaviour
{
    [SerializeField] public Transform player;
    [SerializeField] public float SmoothProperty;
    [SerializeField] public Vector3 offset;
    [SerializeField] public float LejaniaCentral;

    Vector3 vel;

    // Update is called once per frame

    void Update()
    {
        float HD = Input.GetAxisRaw("HorizontalDIRECCION");
        float VD = Input.GetAxisRaw("VerticalDIRECCION");
        if (Mathf.Abs(HD) + Mathf.Abs(VD) != 0)
        {
            offset.x = HD;
            offset.y = VD;
        }
        else
        {
            offset.x = Input.GetAxisRaw("Horizontal");
            offset.y = Input.GetAxisRaw("Vertical");
        }
        Vector3 target = player.position + (offset * LejaniaCentral);
        transform.position = Vector3.SmoothDamp(transform.position, target, ref vel, SmoothProperty);
    }
}