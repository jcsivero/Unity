using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTheGuide : MonoBehaviour
{
    [SerializeField]
    private Transform goal_;
    [SerializeField]
    private float speed_  = 2f;

/// <summary>
/// Start is called on the frame when a script is enabled just before
/// any of the Update methods is called the first time.
/// </summary>
void Start()
{
    if (goal_ == null)        
        goal_= GameObject.Find("Guide").transform;
    
    
}   
void FollowAndTranslateTo(Transform draft)
{
    Vector3 goal = draft.position - transform.position;
    //transform.LookAt(draft.position);

    //Sobre todos los ejes suavizado. Notar que aquí LockRotation utiliza un vector dirección para saber hacia donde rotar,y no la posicion en las coordenadas del mundo
    //del objeto a mirar, como si sucede en LookAt()
    
    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(goal), speed_ * Time.deltaTime);
    
    
    transform.Translate(goal * Time.deltaTime);
    Debug.DrawRay(transform.position, goal, Color.red);    
}
/// <summary>
/// LateUpdate is called every frame, if the Behaviour is enabled.
/// It is called after all Update functions have been called.
/// </summary>
void Update()
{
    FollowAndTranslateTo(goal_);
}
    

}
