using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
////clase genérica de datos para almacenar cualquier tipo de valor. Después se puede heredar de esta clase y por polimorfismo realizar diferentes operaciones
///Además se podría hacer CASTING si se sabe el tipo que se quiere tratar y por lo tanto utilizar todos sus métodos propios.
public class GenericData 
{
        [SerializeField]
        private object data_;
        private bool hasChanged; //si ha cambiado la variable puesto que se ha realizao un Set, se activa. Se activa en cada Set() no comprueba si realmente ha cambiado su valor

        private GenericData(object value)
        {
            data_ =  value;   
            hasChanged = true;                                            

        }
        public static GenericData Create<T>(T value)
        {
            GenericData data = new GenericData(value);    
            
            return data;
        }    

        public void Set<T>(T value) 
        {
            data_ = value;      
            hasChanged = true;      
        }

        public void ClearFlagChanged()
        {
            hasChanged  = false; ///restablezco el flag de cambiada la variable
        }
        public T GetValue<T>()
        {        
            return (T)data_;
        }

 

}
