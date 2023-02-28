using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////clase genérica de datos para almacenar cualquier tipo de valor. Después se puede heredar de esta clase y por polimorfismo realizar diferentes operaciones
///Además se podría hacer CASTING si se sabe el tipo que se quiere tratar y por lo tanto utilizar todos sus métodos propios.
public class GenericData 
{
        private object data_;

        private GenericData(object value)
        {
            data_ =  value;                           

        }
        public static GenericData Create<T>(T value)
        {
            GenericData data = new GenericData(value);            
            return data;
        }    

        public void Set<T>(T value) 
        {
            data_ = value;            
        }

        public T GetValue<T>()
        {        
            return (T)data_;
        }

 

}
