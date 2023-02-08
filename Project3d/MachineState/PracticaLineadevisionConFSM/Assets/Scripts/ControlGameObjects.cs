using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlGameObjects : BaseMono
{
    public bool isWaypoints_= false;
    public string[] tagsForWayPoints_;    
    public int numberOfOrderThisWayPoint_;
    
    public bool isHidePlace = false;
    public string[] tagsForHidePoints_;

    public string[] tags_; //para poder establecer varias etiquetas a un objeto. Simulando Unreal, puesto que Unity solo permite una etiqueta por
    //objeto
    
    
    void Awake()
    {
        if (tagsForWayPoints_.Length == 0)
        {
            tagsForWayPoints_ = new string[1]; ///si no se han indicado etiquetas, creo una etiqueta llamada WayPoints
            tagsForWayPoints_[0] = "WayPoints";
            
        }
            
        if (tagsForHidePoints_.Length == 0)
        {
            tagsForHidePoints_ = new string[1]; ///si no se han indicado etiquetas, creo una etiqueta llamada HidePoints
            tagsForHidePoints_[0] = "HidePoints";
            
        }
        if (tags_.Length == 0)
        {
            tags_  = new string[1];
            tags_[0] = "Untagged";            
        }

    }


}
