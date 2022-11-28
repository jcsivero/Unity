
using UnityEngine;
using UnityEngine.UI;

public class HudController : MonoBehaviour
{
    [SerializeField] public Text pointsText_,vTraslationText_,vRotationText_,nEnemiesText_,nObjectsText_;
    
    
    [SerializeField] public static float vTraslation_ = 30;
    [SerializeField] public static float vRotation_ = 100;
    [SerializeField] public int nEnemies_=0;
    [SerializeField] public int nObjects_=0;
    [SerializeField] public int points_=1000;
    // Start is called before the first frame update
    void Start()
    {
        if (pointsText_ == null)
        {
            GameObject draft = GameObject.Find("Points");
            pointsText_ = draft.GetComponent<Text>();
        }
            
        if (vTraslationText_ == null)
        {
            GameObject draft = GameObject.Find("VTraslation");
            vTraslationText_ = draft.GetComponent<Text>();
        }
        
        if (vRotationText_ == null)
        {
            GameObject draft = GameObject.Find("VRotation");
            vRotationText_ = draft.GetComponent<Text>();
        }
        
        if (nEnemiesText_ == null)
        {
            GameObject draft = GameObject.Find("NEnemies");
            nEnemiesText_ = draft.GetComponent<Text>();
        }
        
        if (nObjectsText_ == null)
        {
            GameObject draft = GameObject.Find("NObjects");
            nObjectsText_ = draft.GetComponent<Text>();
        }
        //registro mi evento
        GameEvents.OnHudUpdate_ += HudUpdate;
        GameEvents.TriggerOnHudUpdate(vTraslation_,vRotation_,points_,nEnemies_,nObjects_); //actualizo el HUD
    }

void HudUpdate(float vTraslation, float vRotation, int points,int nEnemies,int nObjects)
{
    //valores que se sumaran a los valores actuales.
    ///con 0 indico que no se modifique el valor existente
    if (vTraslation!= 0)
    {
        vTraslation_ += vTraslation;
        vTraslationText_.text = "VTranslation: " + vTraslation_.ToString();
    }    
        

    if (vRotation!= 0)
    {
        vRotation_ += vRotation;
        vRotationText_.text = "VRotation: " + vRotation_.ToString();
    }
        
    
    if (points!= 0)
    {
        points_ += points;
        pointsText_.text = "Puntos: " + points_.ToString();
    }
        
    
    if (nEnemies!= 0)
    {
        nEnemies_ += nEnemies;
        nEnemiesText_.text = "Enemigos Restantes: " + nEnemies_.ToString();
    }
    
    if (nObjects!= 0)
    {
        nObjects_ += nObjects;
        nObjectsText_.text = "Número de Objetos: " + nObjects_.ToString();
    }
        
}
public void IncSpeedTraslation()
{    
    GameEvents.TriggerOnHudUpdate(5,0,0,0,0); //actualizo el HUD
}

public void DecSpeedTraslation()
{
    GameEvents.TriggerOnHudUpdate(-5,0,0,0,0); //actualizo el HUD
}

public void IncSpeedRotation()
{    
    GameEvents.TriggerOnHudUpdate(0,5,0,0,0); //actualizo el HUD
}

public void DecSpeedRotation()
{
    GameEvents.TriggerOnHudUpdate(0,-5,0,0,0); //actualizo el HUD
}

}
