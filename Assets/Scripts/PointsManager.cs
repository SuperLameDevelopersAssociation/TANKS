using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;

public class PointsManager : NetworkBehaviour 
{
    SyncListInt kills;
    SyncListInt deaths;

    public Text output;
	[HideInInspector]
	public bool deathmatchActive;
	Deathmatch deathmatch;

	int killAmount;
    int playerIndex;

    [Server]
	void Start()
	{
        if (deathmatchActive)
        {
            deathmatch = GameObject.FindGameObjectWithTag("DeathMatch").GetComponent<Deathmatch>();
        }

        for(int i = 0; i < NetworkManager.singleton.numPlayers; i++)
        kills.Add(0);
        deaths.Add(0);

        RpcUpdateText();
    }

    [Command]
    public void CmdAwardPoints(int indexKiller, int indexKilled)
    {
        kills[indexKiller]++;
        deaths[indexKilled]++;
        RpcUpdateText();

		if (deathmatchActive) 
		{
			if (kills [indexKiller] >= deathmatch.killsToWin) 
			{
				deathmatch.RpcEndMatch ();
			}
		}
    }

    [ClientRpc]
    public void RpcUpdateText()
    {
        output.text = "";
        for (int index = 0; index < NetworkManager.singleton.numPlayers; index++)
        {
            output.text += "Player: " + (index + 1) + ". Kills: " + kills[index] + " Deaths: " + deaths[index] + "\n";
        }
    }

    [Server]
    public int PlayerThatWon()
    {
        for (int i = 0; i < kills.Count; i++)
        {
            int lastAmt = kills[i];
            if (lastAmt > killAmount)
            {
                playerIndex = i;
                killAmount = lastAmt;
            }
            else if (lastAmt == killAmount)
            {
                return 100; //This just means its a tie.
            }
        }
        return playerIndex;
     }
}
