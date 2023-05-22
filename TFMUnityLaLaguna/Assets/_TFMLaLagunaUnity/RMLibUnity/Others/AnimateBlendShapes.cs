//Using C#
 
using UnityEngine;
using System.Collections;
 
public class AnimateBlendShapes : MonoBehaviour
{
 
       int blendShapeCount;
       public SkinnedMeshRenderer skinnedMeshRenderer;
       public Mesh skinnedMesh;
       public float blendOne = 0f;
       public float blendTwo = 0f;
       public float blendSpeed = 1f;
       public bool blendOneFinished = false;
 
       void Awake ()
       {
          skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer> ();
          skinnedMesh = GetComponent<SkinnedMeshRenderer> ().sharedMesh;
       }
 
       void Start ()
       {
          blendShapeCount = skinnedMesh.blendShapeCount; 
          Debug.Log("Número de shapes: " + blendShapeCount.ToString());
       }
 
       void Update ()
       {
          if (blendShapeCount > 2) {
 
                 if (blendOne < 100f) {
                    skinnedMeshRenderer.SetBlendShapeWeight (0, blendOne);
                    blendOne += blendSpeed;
                 } else {
                    blendOneFinished = true;
                 }
 
                 if (blendOneFinished == true && blendTwo < 100f) {
                    skinnedMeshRenderer.SetBlendShapeWeight (1, blendTwo);
                    blendTwo += blendSpeed;
                 }
 
          }
       }
}
