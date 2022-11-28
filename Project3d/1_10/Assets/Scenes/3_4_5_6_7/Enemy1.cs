using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    [SerializeField]    public Vector3 myPosition_;
    [SerializeField]    private float speed_ = 2.0f;
    [SerializeField]    private float rotationSpeed_ = 10.0f;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        myPosition_ = transform.position; 
        
    }
    // Start is called before the first frame update    
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
       if (transform.position == myPosition_) //solo por si no he movido el objeto en tiempo de ejecución desde el editor-
       {
            float rotation = Input.GetAxis("Horizontal") * rotationSpeed_;
            float translation = Input.GetAxis("Vertical") * speed_;          
            if (rotation !=0 )
            {
                Debug.Log("cambiando rotación");
                transform.Rotate(0,rotation* Time.deltaTime,0);
            }
                
                
            if (translation !=0)
            {
                Debug.Log("cambiando posición");
                transform.Translate(0,0,translation * Time.deltaTime);            
            }
                
            
       }
        //actualizo posición por si se cambia durante la ejecución
        myPosition_ = transform.position;  


    }
}
