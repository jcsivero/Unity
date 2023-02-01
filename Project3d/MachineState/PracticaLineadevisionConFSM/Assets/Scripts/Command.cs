using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command : MonoBehaviour
{
    public GameObject var; 
    public AIController var2;
    public World var3;
    public System.Object var4;
    public int  valor = 10;
    public Material material;
    public MonoBehaviour scripts;
    public abstract bool Exec(StatusWorld obj, EventData data = null);
}
 