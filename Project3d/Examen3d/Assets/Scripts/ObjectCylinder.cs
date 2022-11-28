using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCylinder : MonoBehaviour
{
    private bool isFirstUpdate_ = true;
    private bool isErasered = false;

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.OnActionGameObject_ +=  action;
    }

    // Update is called once per frame
    void Update()
    {
      //actualizo mi hud ahora que estoy seguro que todos los start se han ejecutado y es el primer update de este objeto.
        if (isFirstUpdate_)
        {
            isFirstUpdate_ = false;
            GameEvents.TriggerOnHudUpdate(0,0,0,0,1); //agrego un nuevo objecto a la escena
        }
        
    }

    void action(GameObject character, GameObject other)
    {
        if ((other.gameObject == this.gameObject) && (!isErasered)) ///solo si es este objeto
        {
            isErasered = true;
            gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
            GameEvents.TriggerOnHudUpdate(100,0,-50,0,-1); // incrementa 100 de velocidad pero resta 50 puntos. el objeto se resta

            
        }
    }
}
