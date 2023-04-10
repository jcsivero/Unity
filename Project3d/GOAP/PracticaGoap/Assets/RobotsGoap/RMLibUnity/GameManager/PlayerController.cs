using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour

{
    public CharacterController controller_;
    public Vector3 playerVelocity_;
    public float jumpHeight_;
    public float speed_=2.0f;
    public float gravity_ = -9.81f;
    // Start is called before the first frame update
    void Start()
    {
        controller_ = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

        if (controller_.isGrounded && playerVelocity_.y < 0)
        {
            playerVelocity_.y = 0f;
        }

        if (Input.GetButtonDown("Jump") && controller_.isGrounded)
        {
            playerVelocity_.y += Mathf.Sqrt(jumpHeight_ * -2.0f * gravity_);
        }

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        move = move *Time.deltaTime * speed_;
        
        Vector3 newPosition = transform.TransformPoint(move);
        //controller_.Move(newPosition);
        Debug.Log("value Nueva Posición " +move.ToString() + " " + newPosition.ToString());
        transform.position += newPosition;
        

      /*  if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }*/

        playerVelocity_.y += gravity_ * Time.deltaTime;
        controller_.Move(playerVelocity_ * Time.deltaTime);
    }


}
