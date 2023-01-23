using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed_ =15f;
    private float movMov_ = 6.4f;

        // Update is called once per frame
    void FixedUpdate()
    {        

        float moveInputx = Input.GetAxisRaw("Horizontal");
        float moveInputy = Input.GetAxisRaw("Vertical");        
        moveInputx = moveInputx * moveSpeed_ * Time.deltaTime;   
        moveInputy = moveInputy * moveSpeed_ * Time.deltaTime;
        transform.Translate(moveInputx,moveInputy,0,Space.World);
        
        float moveRotation = Input.GetAxisRaw("Fire1");        
        transform.Rotate(0,0,moveRotation * moveSpeed_ *Time.deltaTime * 5 );

    //playerPosition.x = Mathf.Clamp(playerPosition.x + moveInputx * moveSpeed_ * Time.deltaTime, -movMov_, movMov_);            
        //playerPosition.y = playerPosition.y +  moveInputy * moveSpeed_ * Time.deltaTime;
        
     /*   if (!GameManager.gameManager_.isBallMoving_)
        {
            Vector2 playerPosition =  transform.position;
            playerPosition.y += 0.8f;
            GameObject.Find("Ball").transform.position = playerPosition;
        }*/
            

        //GameObject.Find("Ball").transform.parent = this.gameObject.transform;
        
        

        //transform.position = playerPosition;
        //transform.position += new Vector3(moveInput * moveSpeed_ * Time.deltaTime,0f,0f);
        //transform.Translate(new Vector3(moveInput * moveSpeed_ * Time.deltaTime,0f,0f));
    }
}
