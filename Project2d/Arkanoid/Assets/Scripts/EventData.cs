using System.Collections.Generic;

public class EventDataReturned
{
    public int exitCode_ = 0; //devuelve -1 si se produjo error, si si todo Ok o cualquier valor que se defina según el proyecto. 
    public bool succes_;
    public object objectReturned_;
}
public class EventData
{   
        public string name_;        
        private Dictionary<string,object> data_;

        EventData(string name)
        {
            data_ =  new Dictionary<string,object>();
            name_ = name;                

        }
        public static EventData Create(string name)
        {
            EventData data = new EventData(name);            
            return data;
        }    

        public EventData Set<T>(string key, T value) 
        {
            
            if (!data_.ContainsKey(key))               
                data_.Add(key, value);
            else
                data_[key] = value;

            return this;
        }

        public T Get<T>(string key)
        {
            T result =  default(T);

            if (data_.ContainsKey(key))
                result = (T)data_[key];

            return result;
        }

}
