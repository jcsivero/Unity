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

    //[SerializeField] public List<Dictionary<>> wayPoints_;
    public GameObject[] wayPointsNpcRobots_;
    public Dictionary<string,List<GameObject>> wayPoints_;
    public Dictionary<string,List<GameObject>> hidePoints_;
    public Dictionary<string,List<GameObject>> allGameObjects_;
    
    
 
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
        allGameObjects_ = new Dictionary<string,List<GameObject>>();

        
        
        Debug.Log("|||||||||||||| Awake StatusWorld||||||||||||||||");
        
    }    

    void GetInformation()
    {
        ///Obtengo y clasifico la información de los GameObjects ya cargados.
        GameObject[] draft = GameManager.FindObjectsOfType<GameObject>();
        for (int i=0; i<draft.Length;i++)
        {

        }
    }
    public new void Start()
    {
        Debug.Log("|||||||||||||| Start StatusWorld||||||||||||||||");
        if (GetTarget()== null)
            SetTarget(GameObject.FindGameObjectWithTag("Player")); ///si no se ha establecido un objeto destino, por defecto para los NPC es el GameObject con etiqueta "Player"
        
        //AppendCommand(commandUpdateWayPoints_);


    }


    private void InstaciateCommands()
    {
             commandUpdateWayPoints_ = new CommandUpdateWayPoints();

    }
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Funciones exclusivas  de esta clase
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////        

}
