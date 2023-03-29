using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlGameObjects : BaseMono
{
    //public bool isWaypoints_= false;
    public string[] tagsForWayPoints_;    
    public int indexWayPoint_;
    
    //public bool isHidePoint = false;
    public string[] tagsForHidePoints_;

    public string[] tagsGeneral_; //para poder establecer varias etiquetas a un objeto. Simulando Unreal, puesto que Unity solo permite una etiqueta por
    //objeto
    
    
    override public void Awake()
    {
        base.Awake();

    }
    override public void Start()
    {
        base.Start();
    }


}
