using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class InputController : BaseMono

{
    [Header("Common Values")]
    public bool newInputSystem_ = true;
    public CharacterController characterControllerPlayer_;
    public Camera cameraController_;
    
    [Header("Movement Options")]
    public float jumpHeight_= 2;
    public float speed_=3.0f;

    public float sensitivyX = 80;

    public float sensitivyY = 50;
    
    public float gravityMultiplier_ = 1;
    [Header("Actual Values")]
    
    public float mouseX_;
    public float mouseY_;
    public float moveX_;
    public float moveY_;
    public bool escape_;
    public bool attack_;
    
    private float gravity_;
    private float xRotation_;

    private Vector3 playerVelocity_;
    private MapActions mapActions_;
    
    override public void Awake()
    {
        base.Awake();
        SetName("InputController");      
        Debug.Log("|||||||||||||| Awake + " + GetName().ToString() +"||||||||||||||||");        
        

    }

  
    // Start is called before the first frame update
    override public void Start()
    {
        base.Start();
        Debug.Log("|||||||||||||| Start + " + GetName().ToString() +"||||||||||||||||");
        
         InstaciateCommands();  

       if (mapActions_ == null)
            mapActions_=new MapActions();
        
        mapActions_.Enable();
        mapActions_.Movement.Enable();
        
        if (GetGameManager().mobilVesion_) ///la sensibilidad para versiones móviles la divido entre 3, porque si no, sería demasiado movimiento para los gestos
        {
            sensitivyX /= 5;
            sensitivyY /= 5;
        }

            
        
    }

    // Update is called once per frame
    void Update()
    {
        gravity_ = gravityMultiplier_ * -9.84f;
        if (characterControllerPlayer_ != null)
        {
            InputJump();
            InputMove();
            InputMoveMouse();
            InputAttack();
            InputEscape();

            characterControllerPlayer_.transform.Rotate(Vector3.up * mouseX_);

            characterControllerPlayer_.Move( characterControllerPlayer_.transform.forward * moveY_ + characterControllerPlayer_.transform.right * moveX_);                
            playerVelocity_.y += gravity_ * Time.deltaTime;
            characterControllerPlayer_.Move(playerVelocity_ * Time.deltaTime);
        }
        

        if (cameraController_!= null)
        {
            xRotation_ -= mouseY_;
            xRotation_ = Mathf.Clamp(xRotation_,-90,90);
            cameraController_.transform.localRotation = Quaternion.Euler(xRotation_,0,0);
        }



    }

  //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Funciones exclusivas  de esta clase
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
    private void InstaciateCommands()
    {

    }
    public void  InputMove()
    {
        if (newInputSystem_)
        {
            moveX_ = mapActions_.Movement.Move.ReadValue<Vector2>().x* Time.deltaTime * speed_;
            moveY_ = mapActions_.Movement.Move.ReadValue<Vector2>().y* Time.deltaTime * speed_;
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

            mouseX_ = mapActions_.Movement.Look.ReadValue<Vector2>().x *sensitivyX*speed_ * Time.deltaTime;
            mouseY_ = mapActions_.Movement.Look.ReadValue<Vector2>().y *sensitivyY*speed_ * Time.deltaTime;
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
        if (characterControllerPlayer_.isGrounded && playerVelocity_.y < 0)
            playerVelocity_.y = 0f;

        if (newInputSystem_)
            jumpActions = mapActions_.Movement.Jump.WasPressedThisFrame();
        else
            jumpActions = Input.GetButtonDown("Jump");

        if ( jumpActions && characterControllerPlayer_.isGrounded)
            playerVelocity_.y += Mathf.Sqrt(jumpHeight_ * -2.0f * gravity_ );

    }

    public bool InputEscape()
    {
        if (newInputSystem_)
            escape_=  mapActions_.General.Cancel.WasPressedThisFrame();
        
        else
            escape_ = Input.GetKeyDown(KeyCode.Escape);

        return escape_;
    }

     public bool  InputAttack()
    {
        if (newInputSystem_)
            attack_ = mapActions_.Movement.Attack.WasPressedThisFrame();
            
        else  
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
                attack_ = true;
            else
                attack_ = false;
        }      
        
        return  attack_;
    }
}
