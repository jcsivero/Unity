using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlGameObjects : BaseMono
{
    public bool isWaypoints_= false;
    public string[] tagsForWayPoints_;    
    public int indexThisWayPoint_;
    
    public bool isHidePoint = false;
    public string[] tagsForHidePoints_;

    public string[] tagsGeneral_; //para poder establecer varias etiquetas a un objeto. Simulando Unreal, puesto que Unity solo permite una etiqueta por
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
        if (tagsGeneral_.Length == 0)
        {
            tagsGeneral_  = new string[1];
            tagsGeneral_[0] = "Untagged";            
        }

    }


}
