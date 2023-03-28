using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : BaseMono
{

    ////Iniciación de comandos Comandos que no requieren de parámetros o bien se inicializan con un único parámetro en el contructor,
    /// por lo que se puede solo generar una única instancia de ellos y reutilizarla tantas veces como se quiera.
    bool debugMode_;
    

 //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Variables públicas propias de esta clase
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////        

 //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Variables privadas propias de esta clase
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////      
    [Header("Attributtes")]
    [SerializeField] private int lifesPlayer_;
    //[SerializeField] private int healthPlayer_;
    [SerializeField] public int  numberOfLevels_;
    [SerializeField] public int countEnemies_ = 0;    
    [SerializeField] public int activeLevel_; //por defecto comienza en la escena 1. La 0 es el menú principal
    [SerializeField] public int totalPoints_ = 0;
    [SerializeField] public int  levelPoints_ = 0;

    
    public GoapStates worldStates_;
    //private static Queue<GameObject> patients;
    //private static Queue<GameObject> cubicles;
    
    
 
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Métodos Sobreescritos
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
 


//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Eventos  de esta clase
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////    
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    public  void Awake()
    {
        
       
        ///Inicializo variables de control de objecots.        

        worldStates_ = new GoapStates();
        
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
            Debug.Log("|||||||||||||| Start World||||||||||||||||");

        //if (GetTarget()== null)
          //  SetTarget(GameObject.Find("Player")); ///si no se ha establecido un objeto destino, por defecto para los NPC es el GameObject con etiqueta "Player"
                
        //gameObjectsByName_["HudFinal"][0].SetActive(false);
        //AppendCommand(commandHudUpdateAll_); ///se ejecutará en el primer Update() de GameManager

    }



    
    private void InstaciateCommands()
    {
     
                

    }
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Funciones exclusivas  de esta clase
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////        
    public int GetCountEnemies()
    {
        return countEnemies_;
    }
    public void SetOrAddCountEnemies(int draft)
    {
        countEnemies_ += draft;
    }

    public int GetTotalPoints()
    {
        return totalPoints_;
    }
    public void SetOrAddTotalPoints(int draft)
    {
       totalPoints_ += draft;
    }
    /*override public void  SetHealth(int health)
    {        
        GetLevelManager().ac
        GetTarget().GetComponent<Status>().SetHealth(health);
    }
    override public int  GetHealth()
    {        
        return GetTarget().GetComponent<Status>().GetHealth();
    }*/
    public GoapStates GetGoapStates()
    {
        return worldStates_;
    }    

    /*public void AddPatient(GameObject p)
    {
        patients.Enqueue(p);
    }
    public GameObject RemovePatient()
    {
        if (patients.Count == 0) return null;
        return patients.Dequeue();
    }
    public void AddCublicle(GameObject p)
    {
        cubicles.Enqueue(p);
    }
    public GameObject RemoveCubicle()
    {
        if (cubicles.Count == 0) return null;
        return cubicles.Dequeue();
    }    */
}
