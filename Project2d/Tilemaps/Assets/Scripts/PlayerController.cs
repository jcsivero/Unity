using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject possed_;
    [SerializeField] bool physicEnabled_ = true; ///para indicar si quiero mover con los métodos basados en física o mediante métodos transform.
    [SerializeField] float forceJump_ = 200;    
    [SerializeField] public float moveSpeedX_ = 10;
    [SerializeField] public float moveSpeedY_ = 10;

    public bool isJumping=false;
    public bool isAttacking=false;
    public bool isWalking = false;
    
    public bool isIdle=true;
    

    private Rigidbody2D rigidbody2DOfpossed_;
    private SpriteRenderer spriteRendererOfpossed_;
    private Animator animatorOfpossed_;

    // Start is called before the first frame update
    void Start()
    {
        /* if (possed_ == null)   
            possed_ = GameObject.Find("Character");
        */
        possed_ = gameObject;

        rigidbody2DOfpossed_ = possed_.GetComponent<Rigidbody2D>();
        spriteRendererOfpossed_ = possed_.GetComponent<SpriteRenderer>();
        animatorOfpossed_ = possed_.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
             float moveX = Input.GetAxisRaw("Horizontal") *  moveSpeedX_ * Time.deltaTime;   
             float moveY = Input.GetAxisRaw("Vertical") *  moveSpeedY_* Time.deltaTime;   
             

             isJumping = Input.GetKeyDown(KeyCode.Space);

             if ((isJumping) && (physicEnabled_))
                rigidbody2DOfpossed_.AddForce(new Vector2(0,forceJump_));

             
             isAttacking = Input.GetKeyDown(KeyCode.KeypadEnter);

            if ((isAttacking) && (!isWalking))
            {
                 animatorOfpossed_.SetTrigger("attack");


            }


             if ((isJumping) && (physicEnabled_))
                rigidbody2DOfpossed_.AddForce(new Vector2(0,forceJump_));



             if (moveX != 0)
             {
                
                if (moveX < 0)
                    spriteRendererOfpossed_.flipX = true;
                else 
                    spriteRendererOfpossed_.flipX = false;

               if (isIdle)
               {
                 animatorOfpossed_.SetTrigger("walk");
                 Debug.Log("pasando a walk");
                 isIdle = false;
                 isWalking = true;
               }

               if (physicEnabled_)
               {
                Debug.Log("moviendo con física");
                rigidbody2DOfpossed_.velocity = new Vector2(moveX*30,moveY*30);
               } 
                
                
               else 
                possed_.transform.Translate(moveX,moveY,0); 

             }

             else

             {
               if (isWalking)               
                {
                    animatorOfpossed_.SetTrigger("idle");                                       
                    Debug.Log("pasando a idle");
                    isIdle = true;
                    isWalking = false;
                    if (physicEnabled_)
                        rigidbody2DOfpossed_.velocity = Vector2.zero;

                }
                    
    
             }   
 
             
    }
}
