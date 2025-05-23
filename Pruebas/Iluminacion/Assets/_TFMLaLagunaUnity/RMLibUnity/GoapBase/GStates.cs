﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
///esta clase es para poder serializar un diccionario en el inspector y así poder
//poner precondiciones y efectos desde el mismo. Se utilizará en el inspector como un 
///array de esta de WorldState.
/// En el awake se convierte en un verdadero diccionario.

public class GoapStateInt
{
    public string key;
    public int value;
    
}

[System.Serializable]
public class GoapStateFloat
{
    public string key;
    public float value;
}

[System.Serializable]
public class GoapStateString
{
    public string key;
    public string value;
}

public class GoapStates
{
    private Dictionary<string, GenericData> states_;

    public GoapStates()
    {
        states_ = new Dictionary<string, GenericData>();
        
    }

    public GoapStates(GoapStates cpy)
    {
        states_ = new Dictionary<string, GenericData>(cpy.GetStates());
        
    }


    public bool HasState(string key)
    {
        return states_.ContainsKey(key);
    }

    //esta función habría que utilizarla solo cuando se está seguro de que la variable existe en el diccionario de estados.
    //Lo ideal es utilizar previamente la función GetIfExistState()
    public GenericData GetState(string key)
    {        
            return states_[key];
        
     
    }
    public bool GetIfExistState(string key) ///esta función solo devuelve true si existe la clave, false si no existe. Es  cuando no se necesita
    ///evaluar el valor de la variable, sino simplemente su existencia.
    {
        if (states_.ContainsKey(key))
            return true;
        else
            return false;
    }
    public void SetOrAddState(string key, GenericData value) ///actualizo mis estados con un estado nuevo.
    {
        if (states_.ContainsKey(key))
            states_[key] = value;
        else
            states_.Add(key, value);
    }

    public void SetOrAddStates(GoapStates states) ///actualizo mis estados con otro conjunto de estados.
    {
        foreach (KeyValuePair<string, GenericData> b in states.GetStates())
            if (!states_.ContainsKey(b.Key))
                states_.Add(b.Key, b.Value);        
    }

    public void RemoveState(string key)
    {
        if (states_.ContainsKey(key))
            states_.Remove(key);
    }

 
    public Dictionary<string, GenericData> GetStates()
    {
        return states_;
    }
}
