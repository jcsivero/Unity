using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManagerNivel2 : LevelManager
{
    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();
    }
    override public void Start()
    {
        base.Start();
        GetWorld().SetCountEnemies(0);
             
         gameObjectsByName_["CanvasMobile"][0].SetActive(GetGameManager().mobilVesion_);
         GetLevelManager().gameObjectsByName_["Camera2"][0].SetActive(false); ///desactivo la camara 2
    }
override public void InstaciateCommands()
    {
     /*   base.InstaciateCommands();
        GetCommands().CreateCommand("HudLevelUpdateAll",new CommandHudLevelUpdateAll());
        GetCommands().CreateCommand("HudWorldUpdateAll",new CommandHudWorldUpdateAll());
        GetCommands().CreateCommand("HudUpdateCountEnemies", new CommandHudUpdateCountEnemies());
        GetCommands().CreateCommand("HudUpdateKey", new CommandHudUpdateKey());
        GetCommands().CreateCommand("HudUpdateTotalPoints", new CommandHudUpdateTotalPoints());
*/

    }

}
