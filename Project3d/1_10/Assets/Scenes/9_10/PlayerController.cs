using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public GameObject character_;
    [SerializeField] public float speed_ = 40;
    [SerializeField] public float rotationSpeed_ = 40;
    [SerializeField] public Vector3 oldPos_;
    

    void Start()
    {
        if (character_ == null)   
            character_ = GameObject.Find("Character");
        
    }

    // Update is called once per frame
    void Update()
    {
            if (Input.GetKey(KeyCode.Space))
                speed_ += 1;

            float rotation = Input.GetAxisRaw("Horizontal") * rotationSpeed_;
            if (rotation == 0)
                rotation = Input.GetAxisRaw("Mouse X") * rotationSpeed_;
            
            float translation = Input.GetAxisRaw("Vertical") * speed_;          ;
            //if (translation == 0)
             //   translation = Input.GetAxisRaw("Mouse Y") * speed_;          

            if (rotation !=0 )
            {
             //   Debug.Log("cambiando rotación");
                character_.transform.Rotate(0,rotation* Time.deltaTime,0);
            }

            if (translation !=0)
            {
             //   Debug.Log("posicion actual : " + character_.transform.position);
                
                if (!character_.GetComponent<Character>().collision_)                              
                {
                    oldPos_ = character_.transform.position;                    
                    character_.transform.Translate(0,0,translation * Time.deltaTime); 
                    //Debug.Log("posicion despues de translate  : " + character_.transform.position);
                }
                else
                {
                    //Debug.Log("volviendo aposicion anterior ---------------: " + oldPos_);
                    oldPos_.z = oldPos_.z - 0.1f; // si no es así, no sale del trigger.
                    character_.transform.position=oldPos_;
                    //character_.transform.position = Vector3.zero;
                    
                    
                }
                
                
            }                                
                    
            
    }

 
}
