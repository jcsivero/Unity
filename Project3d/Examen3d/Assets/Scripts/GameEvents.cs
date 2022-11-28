using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public delegate void DelegateTwoParam_(GameObject character, GameObject other);
    public delegate void DelegateFourParam_(float vTraslation, float vRotation, int points,int nEnemies, int nObjects);
    public delegate void Delegate_();

    public static event DelegateFourParam_ OnHudUpdate_;
    public static event DelegateTwoParam_ OnActionGameObject_;
    public static event Delegate_ OnSpider_;
    

    public static void TriggerOnHudUpdate(float vTraslation, float vRotation, int points,int nEnemies,int nObjects)
    {

        OnHudUpdate_(vTraslation,vRotation,points,nEnemies,nObjects);
    }

    public static void TriggerOnActionGameObject(GameObject character,GameObject other)
    {

        OnActionGameObject_(character,other);
        
    }

    public static void TriggerOnSpider()
    {

        OnSpider_();

    }
}
