using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : BaseMono
{


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
    override public  void Awake()
    {
        
       base.Awake();
       SetName("World");      
       Debug.Log("|||||||||||||| Awake + " + GetName().ToString() +"||||||||||||||||");
        
        ///Inicializo variables de control de objecots.        

        worldStates_ = new GoapStates();
        
        /*patients = new Queue<GameObject>();
        cubicles = new Queue<GameObject>();
        GameObject[] cubes = GameObject.FindGameObjectsWithTag("Cubicle");
        foreach (GameObject c in cubes)
            cubicles.Enqueue(c);
        if (cubes.Length > 0)
            world.ModifyState("freeCubicle", cubes.Length);*/


        
        
    }    

    override public void Start()
    {
        base.Start();
        Debug.Log("|||||||||||||| Start + " + GetName().ToString() +"||||||||||||||||");
        InitializeValues(); //inicializo valores de variables y variables GoapStates.
        InstaciateCommands();  
        
        //if (GetTarget()== null)
          //  SetTarget(GameObject.Find("Player")); ///si no se ha establecido un objeto destino, por defecto para los NPC es el GameObject con etiqueta "Player"
                
        //gameObjectsByName_["HudFinal"][0].SetActive(false);
        //AppendCommand(commandHudUpdateAll_); ///se ejecutará en el primer Update() de GameManager

    }



    
    private void InstaciateCommands()
    {
     
                

    }

    private void InitializeValues()
    {
        /*GetWorldStates().SetOrAddState("healthPlayer",GenericData.Create<int>())

                SetValue<int>("healthPlayer",int.Parse(hudTextHealthPlayer_.text));
        SetValue<int>("healthGuard",int.Parse(hudTextHealthGuard_.text));
        SetValue<int>("totalPoints",int.Parse(hudTextTotalPoints_.text));
        SetValue<int>("countenemies",int.Parse(hudTextCountEnemies_.text));*/

        ///Creo e inicializo variables con los valores actuales del Hud, por si fueron puestos desde el inspector.
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
    public void SetCountEnemies(int draft)
    {
        countEnemies_ = draft;
    }
    public int GetTotalPoints()
    {
        return totalPoints_;
    }
     public void SetTotalPoints(int draft)
    {
         totalPoints_ = draft;
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
    override public GoapStates GetWorldStates()
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
