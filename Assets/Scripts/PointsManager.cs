using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using TrueSync;

public class PointsManager : TrueSyncBehaviour 
{
    [AddTracking]
    byte[] kills;
    [AddTracking]
    byte[] deaths;

    public Text output;
	[HideInInspector]
	public bool deathmatchActive;
	Deathmatch deathmatch;

	int killAmount;
    int playerIndex;

	void Start()
	{
        if (deathmatchActive)
        {
            deathmatch = GameObject.FindGameObjectWithTag("DeathMatch").GetComponent<Deathmatch>();
        }
    }

    public override void OnSyncedStart()
    {
        kills = new byte[numberOfPlayers];
        deaths = new byte[numberOfPlayers];
        
        Debug.LogError("The number of Players is: " + numberOfPlayers + " and the length of kills is " + kills.Length);
        UpdateText();
    }

    public void AwardPoints(int indexKiller, int indexKilled)
    {
        kills[indexKiller]++;
        deaths[indexKilled]++;
        UpdateText();

		if (deathmatchActive) 
		{
			if (kills [indexKiller] >= deathmatch.killsToWin) 
			{
				TrueSyncManager.SyncedStartCoroutine (deathmatch.MatchEnding ());
			}
		}
    }

    public void UpdateText()
    {
        output.text = "";
        for (int index = 0; index < numberOfPlayers; index++)
        {
            output.text += "Player: " + (index + 1) + ". Kills: " + kills[index] + " Deaths: " + deaths[index] + "\n";
        }
    }

    public int PlayerThatWon()
    {
        for (int i = 0; i < kills.Length; i++)
        {
            int lastAmt = kills[i];
            if (lastAmt > killAmount)
            {
                playerIndex = i;
                killAmount = lastAmt;
                print("Kill Amt: " + killAmount);
            }
            else if (lastAmt == killAmount)
            {
                return 100; //This just means its a tie.
            }
        }
        return playerIndex;
     }
}
