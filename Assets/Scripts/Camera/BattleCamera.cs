using UnityEngine;
using System.Collections;

public class BattleCamera : MonoBehaviour 
{
    public SpriteRenderer bg;

    public float speedX = 1.0f;


	void Start () 
    {
	}
	
	void Update ()
    {
        float inputX = Input.GetAxis("Horizontal");
        float movementX = inputX * speedX;
        Vector3 movement = new Vector3(movementX, 0, 0);
        transform.position += movement;

	}
}
