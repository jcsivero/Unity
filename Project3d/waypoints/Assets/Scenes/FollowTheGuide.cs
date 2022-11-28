using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTheGuide : MonoBehaviour
{
    [SerializeField]
    private Transform goal_;
    [SerializeField]
    private float speed_  = 1f;

/// <summary>
/// Start is called on the frame when a script is enabled just before
/// any of the Update methods is called the first time.
/// </summary>

void Start()
{
    if (goal_ == null)        
        goal_= GameObject.Find("Guide").transform;
        
    
    
}   
void FollowAndTranslateTo(Transform goal)
{
    Vector3 direcction = goal.position - transform.position;
    transform.LookAt(goal.position);
    
    //Sobre todos los ejes suavizado. Notar que aquí LockRotation utiliza un vector dirección para saber hacia donde rotar,y no la posicion en las coordenadas del mundo
    //del objeto a mirar, como si sucede en LookAt()
    
    //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(goal), speed_ * Time.deltaTime);
    
    //puedo mover así(con coordenadas del munto.) Método estático MoveTowards() de Vector3
    //transform.position = Vector3.MoveTowards(transform.position,goal.position,speed_ * Time.deltaTime);
    
    //o también puedo mover así(con coordenadas del munto.) Conrespecto a un vector dirección
    //transform.position = transform.position + direcction * speed_ * Time.deltaTime;
    
    // o con el método Translate() pero con coordenadas del mundo.
    //transform.Translate(direcction * speed_ * Time.deltaTime,Space.World);

    //Si se quiere hacer con coordenadas locales, (valor por defecto del método Translate()) tener claro los planos sobre los que moverse
    //y que Z, sería ir hacia adelante siempre que este mirando hacia el objeto, cuestión a realizar con LookAt() o similares.
    //este método no se ve influenciado en el cáculo de la velocidad del movimiento por el valor que contengan los valores del vector dirección
    //En los otros métodos, por ejemplo, si Z ya tiene un valor, aumenta la velocidad y si el objeto está más lejos, como Z será mayor, también 
    //se moverá más rápido en función de la velocidad. Con este método, la distancia no importa, siempre Z será speed_*Time.deltaTime.
    
    transform.Translate(0,0, speed_ * Time.deltaTime);

    //Debug.DrawRay(transform.position, direcction, Color.red);    
}
/// <summary>
/// LateUpdate is called every frame, if the Behaviour is enabled.
/// It is called after all Update functions have been called.
/// </summary>
void LateUpdate()
{
    FollowAndTranslateTo(goal_);
}
    

}
