using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TrueSync;
using Photon;

public class PlayerManager : PunBehaviour 
{
	public int tank;
	public static List<GameObject> players;
	public int playerID;
	bool spawned;

	void Start()
	{
		players = new List<GameObject> (PhotonNetwork.playerList.Length);
	}

	void Update()
	{
		if (TrueSyncManager.allPlayerGameObjects != null && !spawned) 
		{
			spawned = true;
			SpawnPlayers ();
		}
	}

	public void SetUpPlayers(PhotonPlayer pp, int tankYouChoose)
	{
		tank = tankYouChoose;
	}

	public void SpawnPlayers()
	{
		for (int i = 0; i < TrueSyncManager.allPlayerGameObjects.Count; i++) 
		{
			if (TrueSyncManager.allPlayerGameObjects [i].GetComponent<PlayerMovement> ().owner == TrueSyncManager.LocalPlayer) 
			{
				TrueSyncManager.allPlayerGameObjects [i].SetActive (false);
			}

		}

		photonView.RPC("ActivateTank", PhotonTargets.All, playerID, tank);

	}

	[PunRPC]
	public void ActivateTank(int player, int tankChosen)
	{
		int startingPoint = player * 3;
		for (int i = 0; i < 3; i++) 
		{
			if (tankChosen == i)
				TrueSyncManager.allPlayerGameObjects [i + startingPoint].SetActive (true);
			else	
				TrueSyncManager.allPlayerGameObjects [i + startingPoint].SetActive (false);
		}
	}
}
