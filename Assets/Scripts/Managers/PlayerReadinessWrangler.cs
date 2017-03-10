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
	public GameObject[] playerReadinessPanel;

    public override void OnSyncedStart()
    {
		playerReadinessPanel = GameObject.FindGameObjectsWithTag ("ReadyPanel");
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
			foreach (GameObject go in playerReadinessPanel) 
			{
				go.SetActive (false);
			}
			TrueSyncManager.RunSimulation();

			startGameMode.SetActive (true);
            enabled = false;
		}
        else
            readinessCounter = 0;
    }
		
	public override void OnSyncedUpdate()
	{
        CheckReadiness();
	}
}