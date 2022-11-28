using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    public Vector3 positionInitial_;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        positionInitial_ = transform.position; 
    }
    // Start is called before the first frame update    
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
       //actualizo posición por si se cambia durante la ejecución
       positionInitial_ = transform.position;  
    }
}
