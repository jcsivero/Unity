using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public GameObject character_;

    [SerializeField] public Vector3 oldPos_;

    [SerializeField] private GameObject nido1_;

    
    [SerializeField] private Vector3 goal, direction;
    [SerializeField] private float accuracy = 5.0f;
    [SerializeField] private float magnitude;
    void Start()
    {        
        if (character_ == null)   
            character_ = GameObject.Find("Character");
        if (nido1_ == null)
           nido1_ = GameObject.Find("Nido1");

    }

    // Update is called once per frame
    void Update()
    {
         
            float rotation = Input.GetAxisRaw("Horizontal") *  HudController.vRotation_;
            if (rotation == 0)
                rotation = Input.GetAxisRaw("Mouse X") * HudController.vRotation_ * 10; //los valores del mouse son más pequeños, así que los incremento un poco
            
            float translation = Input.GetAxisRaw("Vertical") * HudController.vTraslation_;          ;
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


        goal = (nido1_.transform.position - character_.transform.position);        
        magnitude = goal.magnitude;  //magnitud de la distancia.
        
        if (magnitude < accuracy)
        {
            GameEvents.TriggerOnSpider();
            /// lookat sobre el suelo.            
            //Vector3 draf = new Vector3(character_.transform.position.x,transform.position.y,character_.transform.position.z);
            //transform.LookAt(draf);
/*
            ///Completo, sobre todos los ejes.
            //transform.LookAt(character_.transform.position);

            //Sobre todos los ejes suavizado. Notar que aquí LockRotation utiliza un vector dirección para saber hacia donde rotar,y no la posicion en las coordenadas del mundo
            //del objeto a mirar, como si sucede en LookAt()
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), speed_ * Time.deltaTime);

            ///trasladar solo sobre eje Z  de coordenadas locales           
            //transform.Translate(0,0, speed_* Time.deltaTime,Space.Self);

            //trasladar sobre todos los ejes con coordenadas globales.
            transform.Translate(goal * Time.deltaTime, Space.World);
            Debug.DrawRay(transform.position, direction, Color.red);
*/
        }



    }


}
