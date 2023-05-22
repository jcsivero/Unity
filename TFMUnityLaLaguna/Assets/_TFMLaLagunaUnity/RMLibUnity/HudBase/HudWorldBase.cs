using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudWorldBase : HudBase
{

    override public void Awake()
    {
        base.Awake();
        SetName("HudWorld");      
        Debug.Log("|||||||||||||| Awake + " + GetName().ToString() +"||||||||||||||||");        
        

    }
   override public void Start()
    {
        base.Start();
        Debug.Log("|||||||||||||| Start + " + GetName().ToString() +"||||||||||||||||");
        
         InstaciateCommands();  
        
    }    
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Funciones exclusivas  de esta clase
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
    private void InstaciateCommands()
    {

    }  
}
