using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject possed_;
    private Rigidbody2D rigidbody2DOfpossed_;
    private SpriteRenderer spriteRendererOfpossed_;
    private Animator animatorOfpossed_;

    private bool isWalking = false;
    private bool isAttacking = false;
    private bool isIdle=true;
    
    // Start is called before the first frame update
    void Start()
    {
         if (possed_ == null)   
            possed_ = GameObject.Find("Character");

        rigidbody2DOfpossed_ = possed_.GetComponent<Rigidbody2D>();
        spriteRendererOfpossed_ = possed_.GetComponent<SpriteRenderer>();
        animatorOfpossed_ = possed_.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
             float moveX = Input.GetAxisRaw("Horizontal") *  GameManager.gameManager_.moveSpeedX_ * Time.deltaTime;   
             float moveY = Input.GetAxisRaw("Vertical") *  GameManager.gameManager_.moveSpeedY_* Time.deltaTime;   

             if (moveX != 0)
             {
                rigidbody2DOfpossed_.velocity = new Vector2(moveX*100,moveY*100);
                if (moveX < 0)
                    spriteRendererOfpossed_.flipX = true;
                else 
                    spriteRendererOfpossed_.flipX = false;

               if (isIdle) 
               {
                 animatorOfpossed_.SetTrigger("walk");
                 isIdle = false;
                 isWalking = true;
               }
               
             }

             else

             {
                if (isWalking)
                {
                    animatorOfpossed_.SetTrigger("idle");
                    isIdle = true;
                    isWalking = false;
                }
                    
             }   

            //possed_.transform.Translate(moveX,moveY,0);   
            

             //if (Input.GetKeyDown.(KeyCode.Space))
              //  possed_.transform.Translate(moveX,moveY,0);   
             
             
             


             //float moveY = Input.GetAxisRaw("Jump") *  GameManager.gameManager_.moveSpeedY_* Time.deltaTime;   
             
    }
}
