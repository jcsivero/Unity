using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3 : MonoBehaviour
{
    [SerializeField]    private GameObject character_;

    [SerializeField]    private float speed_ = 2.0f;
    [SerializeField ]   private Vector3 goal,direction;
    [SerializeField ]   private float accuracy = 5.0f;
    [SerializeField ]   private float magnitude;
    private Character myCharacter_;
    
    // Start is called before the first frame update
    void Start()
    {
        //Obtengo referencias a objetos en la escena.
        if (character_ == null)        
            character_ = GameObject.Find("Character");
                    
        //aqui obtengo acceso a los componentes que me interesaan del GameObejct
        
    }

    // Update is called once per frame
    void Update()
    {
        goal = (character_.transform.position - transform.position); 
        direction = goal; //para tener siempre la dirección
        magnitude = goal.magnitude;  //magnitud de la distancia.
        goal = goal.normalized * speed_; //dirección normalizada y aplicada velocidad.

        if (magnitude > accuracy)
        {
            /// lookat sobre el suelo.            
            //Vector3 draf = new Vector3(character_.transform.position.x,transform.position.y,character_.transform.position.z);
            //transform.LookAt(draf);

            ///Completo, sobre todos los ejes.
            //transform.LookAt(character_.transform.position);

            //Sobre todos los ejes suavizado. Notar que aquí LockRotation utiliza un vector dirección para saber hacia donde rotar,y no la posicion en las coordenadas del mundo
            //del objeto a mirar, como si sucede en LookAt()
            transform.rotation= Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(direction),speed_* Time.deltaTime);
            
            ///trasladar solo sobre eje Z  de coordenadas locales           
            //transform.Translate(0,0, speed_* Time.deltaTime,Space.Self);

            //trasladar sobre todos los ejes con coordenadas globales.
            transform.Translate(goal * Time.deltaTime,Space.World);
            Debug.DrawRay(transform.position,direction,Color.red);
        }
            

    }
}
