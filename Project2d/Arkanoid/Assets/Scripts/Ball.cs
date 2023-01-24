using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private Vector2 initialVelocity_;
           
    private Rigidbody2D ballRb_;
    


    // Start is called before the first frame update
    void Start()
    {
        ballRb_ = GetComponent<Rigidbody2D>();
        initialVelocity_ = new Vector2(0,0);
        
                
    }

    void Update()
    {
       if ((Input.GetKeyDown(KeyCode.Space)) && (!GameManager.gameManager_.isBallMoving_))
        {
            transform.parent= null;       
            ballRb_.simulated = true;     
            //if (initialVelocity_ == Vector2.zero)
            //{
                initialVelocity_.x = Random.Range(3f,5f) * GameManager.gameManager_.activeLevel_;
                initialVelocity_.y = Random.Range(3f,5f) * GameManager.gameManager_.activeLevel_;
            //}   
            
            GameManager.gameManager_.isBallMoving_= true;                                                
            

        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {

      if (GameManager.gameManager_.isBallMoving_)   
      {
            ballRb_.velocity=initialVelocity_;
            GameManager.gameManager_.isBallMoving_ = false;
      }
        
    
    }
    

    
}
