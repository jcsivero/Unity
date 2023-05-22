using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedBlendShapesV2 : MonoBehaviour
{
public float animationSpeed_= 0.05f;
public bool animationBetewenBlendShapes_ = false;
public bool blendShapesIncremental = false;
public int blendShapeCount ;
public SkinnedMeshRenderer skinnedMeshRenderer_;
public Mesh meshBase_;
int currentFrame_;
float frameLength;

void Start ()
{
    skinnedMeshRenderer_ = GetComponent<SkinnedMeshRenderer> ();
    meshBase_ = GetComponent<SkinnedMeshRenderer> ().sharedMesh;
    blendShapeCount = meshBase_.blendShapeCount; 
    Debug.Log("Número de shapes: " + blendShapeCount.ToString());
}
void Update()
{
    if (!animationBetewenBlendShapes_)    
    {
        if (frameLength >= animationSpeed_)
        {
            UpdateAnimationFrame();
            frameLength = 0;
        }
        else
        {
            frameLength += Time.deltaTime;
        }

    }
    else
    {
        
    }
}

void UpdateAnimationFrame()
{
    skinnedMeshRenderer_.SetBlendShapeWeight(currentFrame_, 0);
    currentFrame_++;
    if (currentFrame_ > blendShapeCount) Loop();
    skinnedMeshRenderer_.SetBlendShapeWeight(currentFrame_, 100);
}
void Loop()
{
    skinnedMeshRenderer_.SetBlendShapeWeight(currentFrame_ - 1, 0);
    currentFrame_ = 0;
}
}
