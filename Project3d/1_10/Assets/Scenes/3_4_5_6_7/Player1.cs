using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1 : MonoBehaviour
{
    [SerializeField]    private GameObject enemy_;

    [SerializeField]    private float speed_ = 2.0f;
    [SerializeField ]   private Vector3 goal,direction;
    [SerializeField ]   private float accuracy = 2.0f;
    [SerializeField ]   private float magnitude;
    private Enemy1 myEnemy;
    
    // Start is called before the first frame update
    void Start()
    {
        //Obtengo referencias a objetos en la escena.
        if (enemy_ == null)
        {
            enemy_ = GameObject.Find("Enemy1");
            myEnemy = enemy_.GetComponent<Enemy1>();
        }       
        //aqui obtengo acceso a los componentes que me interesaan del GameObejct
        
    }

    // Update is called once per frame
    void Update()
    {
        goal = (myEnemy.myPosition_ - transform.position); 
        direction = goal; //para tener siempre la dirección
        magnitude = goal.magnitude;  //magnitud de la distancia.
        goal = goal.normalized * speed_; //dirección normalizada y aplicada velocidad.

        if (magnitude > accuracy)
        {
            /// lookat sobre el suelo.            
            //Vector3 draf = new Vector3(myEnemy.myPosition_.x,transform.position.y,myEnemy.myPosition_.z);
            //transform.LookAt(draf);

            ///Completo, sobre todos los ejes.
            //transform.LookAt(myEnemy.myPosition_);

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
