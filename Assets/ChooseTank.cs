using UnityEngine;
using System.Collections;

public class ChooseTank : MonoBehaviour 
{
	PlayerManager playerManager;

	void Start () 
	{
		playerManager = GameObject.Find ("GameManager").GetComponent<PlayerManager> ();
	}

	public void ChooseMyTank(int tankIChoose)
	{
		playerManager.SetUpPlayers (PhotonNetwork.player, tankIChoose);
	}
}
