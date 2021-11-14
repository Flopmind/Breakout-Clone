using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BallScript : NetworkBehaviour
{

    [SerializeField]
    private float ballSpeed = 5;
    [SerializeField]
    private float yLowerBound = -5.15f;
    [SerializeField]
    private float yUpperBound = 4.0f;
    [SerializeField]
    private float xBound = 8.5f;
    [SyncVar]
    private bool isLaunched;
    private Vector2 lastVelo;
    private NetworkedPlayerScript myPlayer;

    // Tracks whether or not the ball is in a player's custody.
    public bool Launched
    {
        get { return isLaunched; }
    }

    // The speed the ball will always travel at.
    public float Speed
    {
        get { return ballSpeed; }
    }

    // Regulates access and modification to the ball's velocity.
    public Vector2 Velocity
    {
        get { return GetComponent<Rigidbody2D>().velocity; }
        set
        {
            if (Velocity.magnitude > 0)
            {
                lastVelo = Velocity;
            }
            GetComponent<Rigidbody2D>().velocity = value;
        }
    }

    public NetworkedPlayerScript Player
    {
        get { return myPlayer; }
        set { myPlayer = value; }
    }

    void Start()
    {
        isLaunched = false;
    }

    void Update()
    {
        if (transform.position.y < Mathf.Abs(yLowerBound)
            || transform.position.y > yUpperBound
            || transform.position.x > xBound
            || transform.position.x < -xBound)
        {
            isLaunched = false;
        }

        Velocity = Velocity.normalized * ballSpeed;

        if (Velocity.magnitude == 0 && isLaunched)
        {
            Velocity = lastVelo.normalized * ballSpeed;
        }
    }

    // Launch the ball in a random upward direction, but only if it hasn't already been launched.
    [Command]
    public void Launch()
    {
        if (!Launched)
        {
            isLaunched = true;
            Vector2 launchVector = new Vector2(0, 1);
            Quaternion rotationQuat = Quaternion.Euler(0.0f, 0.0f, Random.Range(-45.0f, 45.0f));
            launchVector = rotationQuat * launchVector;
            Velocity = ballSpeed * launchVector.normalized;
            lastVelo = launchVector.normalized;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Previous attempt at refined ball collision: (Left in only for example, would be deleted in a real project)

        //Debug.Log(collision.gameObject);
        ////Vector2 nextVelo = new Vector2(lastVelo.x + collision.collider.attachedRigidbody.velocity.x, lastVelo.y).normalized * ballSpeed;
        //Vector2 nextVelo = Velocity.normalized * ballSpeed;
        //Vector2 closestPoint = collision.collider.ClosestPoint(transform.position);

        //if (closestPoint == Vector2.left || closestPoint == Vector2.right)
        //{
        //    Debug.Log(closestPoint);
        //    nextVelo *= new Vector2(-1, 0);
        //}
        //if (closestPoint == Vector2.up || closestPoint == Vector2.down)
        //{
        //    Debug.Log(closestPoint);
        //    nextVelo *= new Vector2(0, -1);
        //}

        //Debug.Log(Velocity);
        //Velocity = nextVelo;

    }
}
