using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using TrueSync;

public class PointsManager : TrueSyncBehaviour {

    [AddTracking]
    byte[] kills;
    [AddTracking]
    byte[] deaths;

    public Text output;

    public override void OnSyncedStart()
    {
        kills = new byte[numberOfPlayers];
        deaths = new byte[numberOfPlayers];
        UpdateText();
        Debug.Log("Number of Players: " + numberOfPlayers);
    }

    public void AwardPoints(int indexKiller, int indexKilled)
    {
        kills[indexKiller]++;
        deaths[indexKilled]++;
        UpdateText();
    }

    public void UpdateText()
    {
        output.text = "";
        for (int index = 0; index < numberOfPlayers; index++)
        {
            output.text += "Player: " + (index + 1) + ". Kills: " + kills[index] + " Deaths: " + deaths[index] + "\n";
        }
    }
}
