using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{

    public GameObject bullet_;
    public int lifes_ = 3;

    public int healthPlayer_ = 100; 

    // Start is called before the first frame update
  public Player()
  {
            //var draft = World.Instance;
  }
    void Start()
    {
//        var draft = World.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
         Fire();
        }
    }

    void Fire()
    {
        Vector3 position = transform.forward;
        position.y +=0.5f;
        //position.z *=2;
        //position.x *=2;        
        GameObject b = Instantiate(bullet_,transform.position + position, transform.rotation);
        b.GetComponent<Rigidbody>().AddForce(b.transform.forward * 1000);
    }

    /// <summary>
    /// OnCollisionEnter is called when this collider/rigidbody has begun
    /// touching another rigidbody/collider.
    /// </summary>
    /// <param name="other">The Collision data associated with this collision.</param>
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "bullet")
        {
             healthPlayer_ -=10;
            if (healthPlayer_ <= 0)  
            {
                
            }
        }
    }
}
