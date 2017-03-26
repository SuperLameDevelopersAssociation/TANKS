using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class Deathmatch : NetworkBehaviour
{
	public Text matchTime;
	public int matchTimeInMinutes;
	public byte killsToWin;
	public int matchEndingTime;

	[HideInInspector]
	public bool matchEnding;
	
	float minutes = 5;
	float seconds = 0;
	PointsManager pointsManager;

    [Server]
    void Start()
    {
        pointsManager = GameObject.Find("GameManager").GetComponent<PointsManager>();
        pointsManager.deathmatchActive = true;
        minutes = matchTimeInMinutes;
    }

    [Server]
    void Update()
	{
		if (seconds <= 0) 
		{
			minutes = minutes - 1;
			seconds = 59;
		} 
		else if ((int)seconds >= 0) 
		{
			seconds -= Time.deltaTime;
		}

		if (minutes <= 0 && seconds <= 0) 
		{
            RpcEndMatch ();
		}

        RpcUpdateTimerText();
	}

    [ClientRpc]
    void RpcUpdateTimerText()
    {
        if (!matchEnding)
            matchTime.text = string.Format("{0:#00}:{1:00}", minutes, (int)seconds);
    }

    [ClientRpc]
	public void RpcEndMatch()
	{
		if (!matchEnding) 
		{
			matchEnding = true;

			int player = pointsManager.PlayerThatWon ();
            if (player != 100)
                matchTime.text = "Player " + (player + 1) + " Won!";
            else
                matchTime.text = "Tie!";

            Invoke("EndGame", matchEndingTime);
		}
	}

    void EndGame()
    {
        SceneManager.LoadScene("GameOver");
    }
}