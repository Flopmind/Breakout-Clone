using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BlockScript : NetworkBehaviour
{
    
    private GameObject scoreManager;

    [SerializeField]
    private bool isCeiling = false;
    [SerializeField]
    private bool isWall = false;
    
    void Start()
    {
        scoreManager = GameObject.FindGameObjectWithTag("ScoreManager");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ball"))
        {
            BallScript ball = collision.GetComponent<BallScript>();
            
            Vector2 nextVelo = ball.Velocity.normalized * ball.Speed;
            Vector2 closestPoint = collision.ClosestPoint(ball.transform.position);
            nextVelo += GetComponent<Rigidbody2D>().velocity;
            

            if (isWall || (!isWall && IsWithinThreshhold(GetComponent<Collider2D>().ClosestPoint(ball.transform.position).y, ball.transform.position.y, 0.02f)))
            {
                nextVelo = new Vector2(-1 * nextVelo.x, nextVelo.y);
            }
            else if (isCeiling || (!isCeiling && IsWithinThreshhold(GetComponent<Collider2D>().ClosestPoint(ball.transform.position).x, ball.transform.position.x, 0.1f)))
            {
                nextVelo = new Vector2(nextVelo.x, -1 * nextVelo.y);
            }
            
            ball.Velocity = nextVelo;
            if (CompareTag("Block"))
            {
                scoreManager.GetComponent<ScoreManagerScript>().Score += 100;
                //if (NetworkManager.)
                NetworkManager.Destroy(gameObject);
            }
        }
        

    }

    private bool IsWithinThreshhold(float num1, float num2, float threshhold)
    {
        threshhold = Mathf.Abs(threshhold);
        return Mathf.Abs(num1 - num2) <= threshhold;
    }
}
