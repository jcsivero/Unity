using System.Collections;
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

    public bool HasState(string key)
    {
        return states_.ContainsKey(key);
    }
    public GenericData GetState(string key)
    {
        return states_[key];
    }
    public void SetOrAddState(string key, GenericData value)
    {
        if (states_.ContainsKey(key))
            states_[key] = value;
        else
            states_.Add(key, value);
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
