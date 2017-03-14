using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TrueSync;
using UnityEngine.UI;

public class PlayerReadinessWrangler : TrueSyncBehaviour
{
	public static bool ready;
    byte readinessCounter;
	public GameObject customizationPanel;
	public GameObject startGameMode;
	public float timerLength;
	public Text timeLeft; 
	bool readyToLoad;
    public override void OnSyncedStart()
    {
        TrueSyncManager.PauseSimulation();
    }

    public override void OnSyncedInput()
    {
        byte startTimer = 0;
        TrueSyncInput.SetByte(9, startTimer);
    }

    public override void OnGamePaused()
    {
        //TrueSyncManager.SyncedStartCoroutine(UnPauseTimer());
    }

    public void CheckReadiness()
    {
        for(int i = 0; i < numberOfPlayers; i++)
        {
            if (PhotonNetwork.playerList[i].GetScore() > 0)
                readinessCounter++;
        }

		if (readinessCounter == numberOfPlayers)
		{
			TrueSyncManager.RunSimulation();
            TrueSyncManager.SyncedStartCoroutine(Counter());
            enabled = false;
		}
        else
            readinessCounter = 0;
    }

	IEnumerator Counter()
	{
		for (float i = timerLength; i > 0; i--) 
		{
			timeLeft.text = "" + i;
			yield return 1;
		}

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player"))
        {
           // go.GetComponent<CustomizationManager>().CustomizeTank();
        }
		ready = true;

		startGameMode.SetActive (true);
	}

	public override void OnSyncedUpdate()
	{
        CheckReadiness();
	}
}