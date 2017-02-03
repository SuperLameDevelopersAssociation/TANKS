using UnityEngine;
using System.Collections;

public class TutorialMenu : Photon.PunBehaviour {

	// Use this for initialization
	void Start () {
        PhotonNetwork.ConnectUsingSettings("v1.0");
	}

    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinOrCreateRoom("room1", null, null);
        PhotonNetwork.automaticallySyncScene = true;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 30), "Players " + PhotonNetwork.playerList.Length);

        if(PhotonNetwork.isMasterClient && GUI.Button(new Rect(10, 40, 100, 30), "Start"))
        {
            PhotonNetwork.LoadLevel("Scenes/Game");
        }
    }
}
