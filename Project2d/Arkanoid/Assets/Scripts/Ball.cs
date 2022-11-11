using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private Vector2 initialVelocity_;
        
    private bool isBallMoving_ = false;
    private Rigidbody2D ballRb_;


    // Start is called before the first frame update
    void Start()
    {
        ballRb_ = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if ((Input.GetKeyDown(KeyCode.Space)) && (!isBallMoving_))
        {
            transform.parent= null;            
            if (initialVelocity_ == Vector2.zero)
            {
                initialVelocity_.x = Random.Range(0f,15f);
                initialVelocity_.y = Random.Range(0f,15f);
            }   
            
            isBallMoving_ = true;            
            ballRb_.velocity=initialVelocity_;

        }
    
    }


    
}
