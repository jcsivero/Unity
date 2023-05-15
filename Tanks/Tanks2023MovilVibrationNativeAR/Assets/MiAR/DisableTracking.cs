using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARFoundation.Samples;

public class DisableTracking : MonoBehaviour
{
    public GameObject xrOriginAr_;
    // Start is called before the first frame update

    public XROrigin xrOrigin_;
    public ARRaycastManager arRayCastManager_;
    public MakeAppearOnPlane makeAppearOnPlane_;
    public ARPlaneManager aRPlaneManager_;
    void Start()
    {
        if (xrOriginAr_ != null)
        {
            xrOrigin_ = xrOriginAr_.GetComponent<XROrigin>();   
            arRayCastManager_ = xrOriginAr_.GetComponent<ARRaycastManager>();
            makeAppearOnPlane_= xrOriginAr_.GetComponent<MakeAppearOnPlane>();
            aRPlaneManager_ = xrOriginAr_.GetComponent<ARPlaneManager>();

        }

    }


    public void DisableTrackables()
    {
        arRayCastManager_.enabled = !arRayCastManager_.enabled;
        makeAppearOnPlane_.enabled = !makeAppearOnPlane_.enabled;
        aRPlaneManager_.enabled = !aRPlaneManager_.enabled;
        
        ARCameraManager draft = GameObject.FindWithTag("MainCamera").GetComponent<ARCameraManager>();        
        //ARCameraBackground draft = GameObject.FindWithTag("MainCamera").GetComponent<ARCameraBackground>();
        draft.enabled = !draft.enabled;        
        bool trackablesEnables = draft.enabled;
        if (!trackablesEnables)
            Camera.main.clearFlags = CameraClearFlags.Skybox;
        else
            Camera.main.clearFlags = CameraClearFlags.Nothing;


    }

}
