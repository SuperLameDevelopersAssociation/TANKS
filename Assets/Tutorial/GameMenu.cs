using UnityEngine;
using System.Collections;

public class GameMenu : Photon.PunBehaviour
{
	void Start ()
    {
        print("Connecting to server... ");
        PhotonNetwork.ConnectUsingSettings("v1.0"); //This will connect us to the network and if Auto-Join lobby is check it will try to join lobby and call the OnJoinedLobby() method.
	}
	
	public override void OnJoinedLobby()
    {
        print("Connection Successful!");
        print("Attemping to join lobby...");
        PhotonNetwork.JoinOrCreateRoom("room1", null, null); //This will attempt to join a lobby or create one if one does not exsist.
    }

    void OnGUI()    //Displays how many players are in the room and allows you to load the scene on all players simultaneously
    {
        GUI.Label(new Rect(10, 10, 100, 30), "players: " + PhotonNetwork.playerList.Length);

        PhotonNetwork.automaticallySyncScene = true; //Placed this code here so that all clients load the scene simultaneously
        if (PhotonNetwork.isMasterClient && GUI.Button(new Rect(10, 40, 100, 30), "start"))
        {
            print("Connected to lobby!");
            print("Loading level...");
            PhotonNetwork.LoadLevel(1);
        }
    }
}
