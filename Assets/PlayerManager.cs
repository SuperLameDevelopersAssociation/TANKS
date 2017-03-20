using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TrueSync;

public class PlayerManager : TrueSyncBehaviour 
{
	public List<int> tankChosen;
	public GameObject[] tankOptions;
	public static List<GameObject> players;

	public override void OnSyncedStart()
	{
		players = new List<GameObject> (PhotonNetwork.playerList.Length);
	}

	public void SetUpPlayers(PhotonPlayer pp, int tankYouChoose)
	{
		for (int i = 0; i < PhotonNetwork.playerList.Length; i++) 
		{
			if (pp == PhotonNetwork.playerList [i]) 
			{
				print (i);
				tankChosen [i] = tankYouChoose;
			}
		}
	}

	public void SpawnPlayers()
	{
		for (int i = 0; i < tankChosen.Count; i++) 
		{
			GameObject playerClone = TrueSyncManager.SyncedInstantiate (tankOptions[tankChosen[i]]) as GameObject;
			players.Add (playerClone);
		}
	}
}
