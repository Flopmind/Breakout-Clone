using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkSpawner : NetworkBehaviour
{

    [SerializeField]
    private GameObject ballPrefab;
    [SerializeField]
    private GameObject[] players;

    // Start is called before the first frame update
    void Start()
    {
        players = new GameObject[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (!isServer) return;
        if (GameObject.FindGameObjectsWithTag("Player").Length > players.Length)
        {
            players = GameObject.FindGameObjectsWithTag("Player");
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].GetComponent<NetworkedPlayerScript>() && players[i].GetComponent<NetworkedPlayerScript>().Ball == null)
                {
                    SpawnBall(players[i].GetComponent<NetworkedPlayerScript>());
                }
            }
        }

        // Attempting to make ball hovering and different player heights work across both client and server.
        for (int i = 0; i < players.Length; i++)
        {
            players[i].transform.position = new Vector3(players[i].transform.position.x, players[i].transform.position.y + i);
            if (players[i].GetComponent<NetworkedPlayerScript>().Ball.GetComponent<BallScript>().Launched == true) return;
            players[i].GetComponent<NetworkedPlayerScript>().Ball.transform.position
                = players[i].transform.position + new Vector3(players[i].GetComponent<NetworkedPlayerScript>().BallHover.x, players[i].GetComponent<NetworkedPlayerScript>().BallHover.y);
        }
    }

    // Spawns the ball for the given playerscript and assigns authority.
    private void SpawnBall(NetworkedPlayerScript playerScript)
    {
        GameObject myBall = Instantiate(ballPrefab, new Vector3(playerScript.transform.position.x + playerScript.BallHover.x, playerScript.transform.position.y + playerScript.BallHover.y), Quaternion.identity);
        NetworkServer.Spawn(myBall, playerScript.GetComponent<NetworkIdentity>().connectionToClient);
        myBall.GetComponent<NetworkIdentity>().AssignClientAuthority(myBall.GetComponent<NetworkIdentity>().connectionToClient);
        myBall.GetComponent<NetworkIdentity>().AssignClientAuthority(playerScript.GetComponent<NetworkIdentity>().connectionToClient);
        playerScript.GetComponent<NetworkIdentity>().AssignClientAuthority(myBall.GetComponent<NetworkIdentity>().connectionToClient);
        playerScript.Ball = myBall;
    }
}
