using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CambiarRotacion : MonoBehaviour
{
    private Vector3 objetivo;

    [SerializeField] private Transform player_;

    [SerializeField] private Camera camara;

    // Update is called once per frame
    void Update()
    {
        float HD = Input.GetAxisRaw("HorizontalDIRECCION");
        float VD = Input.GetAxisRaw("VerticalDIRECCION");
        float H = Input.GetAxisRaw("Horizontal");
        float V = Input.GetAxisRaw("Vertical");
        // Con teclado
        objetivo = camara.ScreenToWorldPoint(Input.mousePosition);
        /*
        if (Input.GetJoystickNames()[0] == "") {  }
        // Con mando
        else { 
            if (Mathf.Abs(HD) + Mathf.Abs(VD) != 0) {
                objetivo.x = transform.position.x + HD;
                objetivo.y = transform.position.y + VD;
            } 
            else if (Mathf.Abs(H) + Mathf.Abs(V) != 0) {
                objetivo.x = transform.position.x + H;
                objetivo.y = transform.position.y + V;
            }
            else {
                objetivo.x = transform.position.x + player_.transform.localScale.x;
            }
        }
        */
        float anguloRadianes = Mathf.Atan2(objetivo.y - transform.position.y, objetivo.x - transform.position.x);
        float anguloGrados = (180 / Mathf.PI) * anguloRadianes - 90;
        transform.rotation = Quaternion.Euler(0, 0, anguloGrados);
    }
}