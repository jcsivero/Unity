using System.Collections.Generic;

public class DataExitReturned
{
    public int exitCode_ = 0; //devuelve -1 si se produjo error, si si todo Ok o cualquier valor que se defina según el proyecto. 
    public bool succes_;
    public object objectReturned_;
}
public class CompositeData
{   
    public string name_;        
    private Dictionary<string,object> data_;

    public CompositeData()
    {
        data_ =  new Dictionary<string,object>();
        name_ = "NoName";                

    }
    public CompositeData(string name)
    {
        data_ =  new Dictionary<string,object>();
        name_ = name;                

    }
    public static CompositeData Create(string name)
    {
        CompositeData data = new CompositeData(name);            
        return data;
    }    

    public static CompositeData Create()
    {
        CompositeData data = new CompositeData();            
        return data;
    }    

    public CompositeData Set<T>(string key, T value) 
    {
        
        if (!data_.ContainsKey(key))               
            data_.Add(key, value);
        else
            data_[key] = value;

        return this;
    }

    ///devuelve el valor asociado a la clave y si no existe la clave, devuelve el valor por devecto asociado al tipo de variable que se solicitó, null, cero ...
    public T Get<T>(string key)
    {
        T result =  default(T);

        if (data_.ContainsKey(key))
            result = (T)data_[key];

        return result;
    }

    public bool GetIfExistValue(string key) ///esta función solo devuelve true si existe la clave, false si no existe. Es  cuando no se necesita
    ///evaluar el valor de la variable, sino simplemente su existencia.
    {
        if (data_.ContainsKey(key))
            return true;
        else
            return false;
    }
    public bool Remove(string key)
    {
        if (data_.ContainsKey(key))
            return data_.Remove(key);
        return false;    
    }

}
