using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class PlayerController : MonoBehaviour

{
    public bool newInputSystem_ = false;
    public CharacterController movementController_;
    public Camera cameraController_;
    public float jumpHeight_;
    public float speed_=2.0f;

    public float sensitivyX = 100;

    public float sensitivyY = 100;
    
    public float gravityMultiplier_ = 1;

    
    private float gravity_;
    private float xRotation_;
    private float mouseX_;
    private float mouseY_;
    private float moveX_;
    private float moveY_;
    private Vector3 playerVelocity_;
    private MapActions mapActions_;
    
    // Start is called before the first frame update
    void Start()
    {
        movementController_ = GetComponent<CharacterController>();
        cameraController_ = GetComponentInChildren<Camera>();
        if (cameraController_ == null)
            cameraController_= GameObject.Find("CameraController").GetComponent<Camera>();
        
        mapActions_=new MapActions();
        mapActions_.Enable();
        mapActions_.Movement.Enable();
        
    }

    // Update is called once per frame
    void Update()
    {
        gravity_ = gravityMultiplier_ * -9.84f;

        InputJump();
        InputMove();
        InputMoveMouse();
        xRotation_ -= mouseY_;
        xRotation_ = Mathf.Clamp(xRotation_,-90,90);
        cameraController_.transform.localRotation = Quaternion.Euler(xRotation_,0,0);
        transform.Rotate(Vector3.up * mouseX_);

        movementController_.Move( transform.forward * moveY_ + transform.right * moveX_);                
        playerVelocity_.y += gravity_ * Time.deltaTime;
        movementController_.Move(playerVelocity_ * Time.deltaTime);


    }

  
    public void  InputMove()
    {
        if (newInputSystem_)
        {
            moveX_ = mapActions_.Movement.Move.ReadValue<Vector2>().x;
            moveY_ = mapActions_.Movement.Move.ReadValue<Vector2>().y;
        }
        else
        {
            moveX_ = Input.GetAxis("Horizontal")* Time.deltaTime * speed_;
            moveY_ = Input.GetAxis("Vertical")* Time.deltaTime * speed_; 
            
        }
                
    }

    public void InputMoveMouse()
    {
        if (newInputSystem_)
        {


        }
        else
        {
            mouseX_ = Input.GetAxis("Mouse X")*sensitivyX*speed_ * Time.deltaTime;
            mouseY_ = Input.GetAxis("Mouse Y")*sensitivyY*speed_ * Time.deltaTime;

        }
        
    }
    public void CameraRotation(float axisX,float axisY)
    {

    }
    public void InputJump()
    {
       bool jumpActions = false;
        if (movementController_.isGrounded && playerVelocity_.y < 0)
            playerVelocity_.y = 0f;

        if (newInputSystem_)
            jumpActions = mapActions_.Movement.Jump.WasPressedThisFrame();
        else
            jumpActions = Input.GetButtonDown("Jump");

        if ( jumpActions && movementController_.isGrounded)
            playerVelocity_.y += Mathf.Sqrt(jumpHeight_ * -2.0f * gravity_ );

    }
}
