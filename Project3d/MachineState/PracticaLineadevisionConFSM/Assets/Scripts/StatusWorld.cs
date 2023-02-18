using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusWorld : Status
{
 //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Variables públicas propias de esta clase
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////    
    public CommandUpdateWayPoints commandUpdateWayPoints_;

 //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Variables privadas propias de esta clase
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////      
    [SerializeField] public int  numberOfLevels_;
    [SerializeField] public int countEnemies_;    
    [SerializeField] public int activeLevel_; //por defecto comienza en la escena 1. La 0 es el menú principal
    [SerializeField] public int totalPoints_ = 0;
    [SerializeField] public int  levelPoints_ = 0;

    public Dictionary<string,List<GameObject>> wayPoints_;
    public Dictionary<string,List<GameObject>> hidePoints_;
    public Dictionary<string,List<GameObject>> gameObjectsByName_;
    public List<GameObject> allGameObjects_;    
    
    
 
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Métodos Sobreescritos
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
 


//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Eventos  de esta clase
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////    
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    public new void Awake()
    {
        base.Awake();
        InstaciateCommands();  
        SetName("StatusWorld");        
        
        ///Inicializo variables de control de objecots.        
        wayPoints_= new Dictionary<string,List<GameObject>>();
        hidePoints_ = new Dictionary<string,List<GameObject>>();
        gameObjectsByName_ = new Dictionary<string,List<GameObject>>();
        allGameObjects_ = new List<GameObject>();
        
        
        Debug.Log("|||||||||||||| Awake StatusWorld||||||||||||||||");
        
    }    

    void GetInitialInformation()
    {
        ///Obtengo y clasifico la información de los GameObjects ya cargados.

        ControlGameObjects controlGameObjects;
        List<GameObject> refToGameObjets;
        string tag;
        string nameGameObject;

        ///Primero borro toda la información, para comenzar desde cero.

        wayPoints_.Clear();
        hidePoints_.Clear();
        gameObjectsByName_.Clear();
        allGameObjects_.Clear();   
                 

        GameObject[] refObject = GameManager.FindObjectsOfType<GameObject>();

        for (int i=0; i<refObject.Length;i++)
        {
            allGameObjects_.Add(refObject[i]); ///los gameobjects encontrados los meto uno a uno en la lista. Sus referencias
            
            ////Ahora actualizo el diccionario gameObjectsByName_ con nombre y lista de gameobjects con el mismo nombre.
            
            nameGameObject =refObject[i].name;

            if (!gameObjectsByName_.ContainsKey(nameGameObject))
                gameObjectsByName_.Add(nameGameObject, new List<GameObject>());
        
            refToGameObjets = gameObjectsByName_[nameGameObject];
            refToGameObjets.Add(refObject[i]);

            /////Ahroa actualizo los wayPoints            

            controlGameObjects =  refObject[i].GetComponent<ControlGameObjects>();
            if (controlGameObjects == null)
                Debug.Log("...................Falta componente ControlGameObjects en objeto con nombre: " + nameGameObject);
            else
            {
                //if (controlGameObjects.isWaypoints_)
                if (controlGameObjects.tagsForWayPoints_.Length > 0)
                {
                    for (int j=0; j < controlGameObjects.tagsForWayPoints_.Length;j++)
                    {
                        tag = controlGameObjects.tagsForWayPoints_[j];
                        if (!wayPoints_.ContainsKey(tag))
                                wayPoints_.Add(tag, new List<GameObject>());
                        
                        refToGameObjets = wayPoints_[tag];
                        if (refToGameObjets.Count == 0)
                            refToGameObjets.Add(refObject[i]);     ///si es el primer objeto de la lista lo agrego, no puedo insertar hasta que haya por lo menos un objeto.                   
                        else
                            refToGameObjets.Insert(controlGameObjects.indexWayPoint_,refObject[i]);  
                                                  
                    }
                }
            ////Ahora actualizo los puntos de ocultación.
            
                //if (controlGameObjects.isHidePoint)
                if (controlGameObjects.tagsForHidePoints_.Length > 0)
                {
                    for (int j=0; j < controlGameObjects.tagsForHidePoints_.Length;j++)
                    {
                        tag = controlGameObjects.tagsForHidePoints_[j];
                        if (!hidePoints_.ContainsKey(tag))
                                hidePoints_.Add(tag, new List<GameObject>());
                        
                        refToGameObjets = hidePoints_[tag];
                        refToGameObjets.Add(refObject[i]);                                                

                    }
                }

            }

            

        }
        
    }
    public new void Start()
    {
        Debug.Log("|||||||||||||| Start StatusWorld||||||||||||||||");
        if (GetTarget()== null)
            SetTarget(GameObject.FindGameObjectWithTag("Player")); ///si no se ha establecido un objeto destino, por defecto para los NPC es el GameObject con etiqueta "Player"
        
        GetInitialInformation();
        


    }


    private void InstaciateCommands()
    {
             commandUpdateWayPoints_ = new CommandUpdateWayPoints();

    }
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Funciones exclusivas  de esta clase
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////        

}
