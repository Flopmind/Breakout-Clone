using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerControllerScript : MonoBehaviour
{

    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private GameObject myBall;
    private Vector2 ballHover;

    public GameObject Ball
    {
        get { return myBall; }
        set { myBall = value; }
    }
    
    void Start()
    {

        ballHover = Ball.transform.position - transform.position;
    }
    
    void Update()
    {
        // Player Controls
        if (Input.GetAxis("Horizontal") < 0)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-movementSpeed, 0);
        }
        else if (Input.GetAxis("Horizontal") > 0)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(movementSpeed, 0);
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Ball.GetComponent<BallScript>().Launch();
        }

        if (!Ball.GetComponent<BallScript>().Launched)
        {
            Ball.GetComponent<BallScript>().Velocity = Vector2.zero;
            Ball.transform.position = transform.position + new Vector3(ballHover.x, ballHover.y, 0);
        }
    }
}
