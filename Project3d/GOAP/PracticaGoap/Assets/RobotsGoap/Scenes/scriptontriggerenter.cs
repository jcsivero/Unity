using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptontriggerenter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger detectado " + gameObject.name + " " + other.gameObject.name);
    }
}
