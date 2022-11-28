using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]    private GameObject enemy_;

    [SerializeField]    private float speed_ = 2.0f;
    [SerializeField ]   private Vector3 goal;
    [SerializeField ]   private float accuracy = 2.0f;
    [SerializeField ]   private float magnitude;
    private Enemy myEnemy;
    
    // Start is called before the first frame update
    void Start()
    {
        //Obtengo referencias a objetos en la escena.
        if (enemy_ == null)
        {
            enemy_ = GameObject.Find("Enemy");
            myEnemy = enemy_.GetComponent<Enemy>();
        }       
        //aqui obtengo acceso a los componentes que me interesaan del GameObejct
        

    }

    // Update is called once per frame
    void Update()
    {
        goal = (myEnemy.positionInitial_ - transform.position);
        magnitude = goal.magnitude;
        goal = goal.normalized * speed_;

        if (magnitude > accuracy)
            transform.Translate(goal * Time.deltaTime);

    }
}
