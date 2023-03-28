using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : BaseMono
{
    public bool debugMode_;

    public HudLevel hudLevel_;
    public GameObject actualPlayer_;
    public GameObject actualEnemy_;
    public Dictionary<string,List<GameObject>> wayPoints_;    
    public Dictionary<string,List<GameObject>> hidePoints_;
    public Dictionary<string,List<GameObject>> gameObjectsByName_;
    public List<GameObject> allGameObjects_; 

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Eventos  de esta clase
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////    
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    public void Awake()
    {
                        
        ///Inicializo variables de control de objecots.        
        wayPoints_= new Dictionary<string,List<GameObject>>();        
        hidePoints_ = new Dictionary<string,List<GameObject>>();
        gameObjectsByName_ = new Dictionary<string,List<GameObject>>();
        allGameObjects_ = new List<GameObject>();
        
        
        /*patients = new Queue<GameObject>();
        cubicles = new Queue<GameObject>();
        GameObject[] cubes = GameObject.FindGameObjectsWithTag("Cubicle");
        foreach (GameObject c in cubes)
            cubicles.Enqueue(c);
        if (cubes.Length > 0)
            world.ModifyState("freeCubicle", cubes.Length);*/


        Debug.Log("|||||||||||||| Awake StatusWorld||||||||||||||||");
        
    }    

    public void Start()
    {    
        if (GetGameManager().debugModeForce_ == DebugModeForce_.debug)
            debugMode_ = true;

        if (GetGameManager().debugModeForce_ == DebugModeForce_.noDebug)
            debugMode_ = false;

        InstaciateCommands();  
        
        if (debugMode_)
            Debug.Log("|||||||||||||| Start StatusWorld||||||||||||||||");

        SetLevelManager(this);
        //if (GetTarget()== null)
          //  SetTarget(GameObject.Find("Player")); ///si no se ha establecido un objeto destino, por defecto para los NPC es el GameObject con etiqueta "Player"
        
        GetInitialInformation();
        //gameObjectsByName_["HudFinal"][0].SetActive(false);
        //AppendCommand(commandHudUpdateAll_); ///se ejecutará en el primer Update() de GameManager

    }


void GetInitialInformation()
    {
        ///Obtengo y clasifico la información de los GameObjects ya cargados.

        ControlGameObjects controlGameObjects;
        List<GameObject> refToGameObjets;
        Dictionary<string,List<GameObject>> wayPointsDraft;

        string tag;
        string nameGameObject;

        ///Primero borro toda la información, para comenzar desde cero.

        wayPoints_.Clear();
        hidePoints_.Clear();
        gameObjectsByName_.Clear();
        allGameObjects_.Clear();   
        wayPointsDraft = new Dictionary<string,List<GameObject>>();

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
                if (controlGameObjects.tagsForWayPoints_.Length > 0)
                {
                    for (int j=0; j < controlGameObjects.tagsForWayPoints_.Length;j++)
                    {
                        tag = controlGameObjects.tagsForWayPoints_[j];
                        if (!wayPoints_.ContainsKey(tag))
                                wayPoints_.Add(tag, new List<GameObject>());
                        
                        refToGameObjets = wayPoints_[tag];
                        if (refToGameObjets.Count< controlGameObjects.indexWayPoint_+1)
                            for (int k=refToGameObjets.Count; k < controlGameObjects.indexWayPoint_+1;k++)
                                refToGameObjets.Add(null); ///relleno huecos en caso de que haya waypoints con 
                                ///numero mayor de los elementos en la lista. Después ya se iran rellenando.
                        refToGameObjets[controlGameObjects.indexWayPoint_] = refObject[i];
                    }
                }
            ////Ahora actualizo los puntos de ocultación.
            
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


    private void InstaciateCommands()
    {


    }
}
