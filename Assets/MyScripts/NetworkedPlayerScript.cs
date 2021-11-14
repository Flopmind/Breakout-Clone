using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkedPlayerScript : NetworkBehaviour
{
    [SerializeField]
    private float movementSpeed = 7.5f;
    [SerializeField]
    private Vector2 ballHover = new Vector2(0, 0.4f);
    [SerializeField]
    private float xBound = 8.0f;

    private GameObject myBall;

    public GameObject Ball
    {
        get { return myBall; }
        set
        {
            myBall = value;
            Ball.GetComponent<BallScript>().Player = this;
        }
    }

    public Vector2 BallHover
    {
        get { return ballHover; }
    }

    //void On

    void Update()
    {
        // Return if you are not the local player.
        if (!isLocalPlayer) return;

        // Player Controls that define the players' horizontal boundaries.
        if (Input.GetAxis("Horizontal") < 0 && transform.position.x > (Mathf.Abs(xBound) * -1))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-movementSpeed, 0);
        }
        else if (Input.GetAxis("Horizontal") > 0 && transform.position.x < Mathf.Abs(xBound))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(movementSpeed, 0);
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }

        // Launches the ball.
        if (Input.GetButtonDown("Fire1") && Ball != null)
        {
            Ball.GetComponent<BallScript>().Launch();
        }

        // Stop the ball if needed.
        if (Ball != null && !Ball.GetComponent<BallScript>().Launched)
        {
            Ball.GetComponent<BallScript>().Velocity = Vector2.zero;
        }
    }
}
