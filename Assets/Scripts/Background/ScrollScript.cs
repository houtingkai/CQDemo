using UnityEngine;
using System.Collections;

public class ScrollScript : MonoBehaviour 
{
    public float percent = 0.3f;
    public float SmoothingDelay = 5;
	void Update ()
    {
        float deltaX = Camera.main.transform.position.x * percent;
        Vector3 posNew = new Vector3(deltaX,0,0);
        transform.position =  Vector3.Lerp(transform.position, posNew, Time.deltaTime * SmoothingDelay);
	}
}
