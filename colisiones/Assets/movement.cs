using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    public MapActions mapActions_;

    private Rigidbody rb_;
    public float speed_ = 50;
    public float mouseX_;
    public float mouseY_;
    public float moveX_;
    public float moveY_;

    public float sensitivyX = 80;

    public float sensitivyY = 50;
    // Start is called before the first frame update
    void Start()
    {
        rb_ = GetComponent<Rigidbody>();

        if (mapActions_ == null)
            mapActions_ = new MapActions();
        //mapActions_.Enable();
        mapActions_.Movement.Enable();

    }

    // Update is called once per frame
    private void Update()
    {

        //   transform.position += new Vector3(moveX_, 0, moveY_);
        //rb_.MovePosition(transform.position + new Vector3(moveX_, 0, moveY_));
        InputMove();

    }
    void FixedUpdate()
    {
        

        rb_.MovePosition(transform.position + new Vector3(moveX_, 0, moveY_));

    }

    public void InputMove()
    {

     

        moveX_ = mapActions_.Movement.Move.ReadValue<Vector2>().x * Time.deltaTime * speed_;
        moveY_ = mapActions_.Movement.Move.ReadValue<Vector2>().y * Time.deltaTime * speed_;

    }


}
