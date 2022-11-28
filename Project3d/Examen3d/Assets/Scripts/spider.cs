using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spider : MonoBehaviour
{
    private Vector3 initialPos_,targetPos_;
    private float speed_ = 50;
    private bool move = false;
    private float inc_;
    // Start is called before the first frame update
    void Start()
    {
        initialPos_ = transform.position;   
        targetPos_ = initialPos_;        
        inc_ = 100;
        transform.LookAt(targetPos_);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(0, 0, speed_ * Time.deltaTime);
        if ((inc_ < 25) && (!move))
        {
            transform.Translate(0, 0, speed_ * Time.deltaTime);
            inc_++;
        }
            
        else 
            move = true;
    
        if ((inc_ > 0) && (move))
        {
            transform.Translate(0, 0, speed_ * Time.deltaTime * -1);
            inc_--;
        }            
        else
            move = false;        
    }
}
