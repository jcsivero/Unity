using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    public Transform attachCameraTo_;
    public Camera cameraActive_;
    public bool attachCameraX_ = false;
    public bool attachCameraZ_ = false;
    public bool attachCameraY_ = false;

    private float xRotation_;
    private float yRotation_;
    private float zRotation_;
  /// <summary>
  /// Start is called on the frame when a script is enabled just before
  /// any of the Update methods is called the first time.
  /// </summary>

void Awake()
{
    
    if (attachCameraTo_ ==  null)
        attachCameraTo_ = transform.root;

}
  void Start()
  {
        
  }
  public void SetFollow(Transform toFollow)
  {
    attachCameraTo_ = toFollow;
  }
  public void CameraRotation(Vector3 rotation)
    {

       /* xRotation_ -= rotation.x;
        yRotation_ -= rotation.y;
        zRotation_ -= rotation.z;
        
        xRotation_ = Mathf.Clamp(rotation.x, -90, 90);
        yRotation_ = Mathf.Clamp(rotation.y, -180, 180);
        
        
        if (attachCameraX_)
            attachCameraTo_.localRotation=Quaternion.Euler(xRotation_, 0, 0);

        else
            this.gameObject.transform.localRotation=Quaternion.Euler(xRotation_, 0, 0);


        /*if (attachCameraY_)
            attachCameraTo_.localRotation=Quaternion.Euler(0, yRotation_, 0);
        else
            this.gameObject.transform.localRotation=Quaternion.Euler(0, yRotation_, 0);
*/
//        attach.transform.Rotate(rotation.x,rotation.);
  //          cameraController_.transform.localRotation = Quaternion.Euler(xRotation_, 0, 0);*/
        
    }
}
