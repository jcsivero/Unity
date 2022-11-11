using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed_ =15f;
    private float movMov_ = 6.4f;

        // Update is called once per frame
    void Update()
    {        
        //GameObject.Find("Ball").transform.parent = this.gameObject.transform;
        float moveInput = Input.GetAxisRaw("Horizontal");
        Vector2 playerPosition =  transform.position;
        playerPosition.x = Mathf.Clamp(playerPosition.x + moveInput * moveSpeed_ * Time.deltaTime, -movMov_, movMov_);
        transform.position = playerPosition;
        //transform.position += new Vector3(moveInput * moveSpeed_ * Time.deltaTime,0f,0f);
        //transform.Translate(new Vector3(moveInput * moveSpeed_ * Time.deltaTime,0f,0f));
    }
}
