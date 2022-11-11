 using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Enemies : MonoBehaviour
{          
    [SerializeField] private GameObject[] enemies_;

    
    // Start is called before the first frame update
    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        
    }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        ///Instanciamos objectos
    }
    // Start is called before the first frame update
    void Start()
    {        
        
        Vector3 newPosition = new Vector3(0,0,0);
        System.Random rnd = new  System.Random();

        for (float i=0; i < 2; i++)
        {            
            newPosition.y= 4 - 2*i;    
            for(float j=0; j < 8; j++)
            {                
                
                GameObject enemy = (GameObject) Instantiate(enemies_[Random.Range(0,enemies_.Length)]); 
                newPosition.x = -7;
                newPosition.x += 2*j ;                
                enemy.transform.position = newPosition;
                enemy.transform.parent = gameObject.transform;
                //Debug.Log("Creando instancia enemigo");

            }

        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {

        
        
    }
    
    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
       OnDisable();
    }
}

