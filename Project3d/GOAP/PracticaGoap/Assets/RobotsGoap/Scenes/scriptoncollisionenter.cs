using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptoncollisionenter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision detectacda " + gameObject.name+ " " + collision.gameObject.name);
    }
}
