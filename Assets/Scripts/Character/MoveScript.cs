using UnityEngine;
using System.Collections;

public class MoveScript : MonoBehaviour 
{
    public float speed = 1.0f;

	void Start () 
    {
	
	}
	
	void Update () 
    {
        float xInput = Input.GetAxis("Horizontal");
        float xMovement = xInput * speed;
        Vector3 movement = new Vector3(xMovement,0,0);
        transform.position += movement;

        var dist = (transform.position - Camera.main.transform.position).z;
        var leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).x;
        var rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, dist)).x;
        var topBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).y;
        var bottomBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, dist)).y;

        transform.position = new Vector3(
                  Mathf.Clamp(transform.position.x, leftBorder, rightBorder),
                  Mathf.Clamp(transform.position.y, topBorder, bottomBorder),
                  transform.position.z
                  ); 
	}
}
