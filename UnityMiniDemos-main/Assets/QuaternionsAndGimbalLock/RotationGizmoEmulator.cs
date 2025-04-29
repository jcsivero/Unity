using UnityEngine;

public class RotationGizmoEmulator : MonoBehaviour
{
	public enum CoordSystem { Global, Local }

	public CoordSystem coordSystem;
	public bool x = false;
	public bool y = false;
	public bool z = false;
	public float speed = 30.0f;

	void Update()
	{
		Quaternion currentRotation = transform.rotation;

		if (coordSystem == CoordSystem.Global)
		{
            //currentRotation = Quaternion.AngleAxis(Time.deltaTime * speed, new Vector3(0.0f, 0.0f, 1.0f))* Quaternion.AngleAxis(Time.deltaTime * speed, new Vector3(1.0f, 0.0f, 0.0f))* Quaternion.AngleAxis(Time.deltaTime * speed, new Vector3(0.0f, 0.0f, 1.0f)) * currentRotation;
            if (x)
                currentRotation = Quaternion.AngleAxis(Time.deltaTime * speed, new Vector3(1.0f, 0.0f, 0.0f)) * currentRotation;

            if (y)
				currentRotation = Quaternion.AngleAxis(Time.deltaTime * speed, new Vector3(0.0f, 1.0f, 0.0f)) * currentRotation;
            
            if (z)
				currentRotation = Quaternion.AngleAxis(Time.deltaTime * speed, new Vector3(0.0f, 0.0f, 1.0f)) * currentRotation;
        }
		else
		{
            /*
             * if (x)
                    currentRotation = Quaternion.AngleAxis(Time.deltaTime * speed, transform.right) * currentRotation;
                if (y)
                    currentRotation = Quaternion.AngleAxis(Time.deltaTime * speed, transform.up) * currentRotation;
                if (z)
                    currentRotation = Quaternion.AngleAxis(Time.deltaTime * speed, transform.forward) * currentRotation;
            */


            if (y)
                currentRotation = Quaternion.Euler(0, Time.deltaTime * speed, 0) * currentRotation;
            if (z)
                currentRotation = Quaternion.Euler(0, 0, Time.deltaTime * speed) * currentRotation;
            if (x)
                currentRotation = Quaternion.Euler(Time.deltaTime * speed, 0, 0) * currentRotation;

  
        }

		transform.rotation = currentRotation;
	}
}
