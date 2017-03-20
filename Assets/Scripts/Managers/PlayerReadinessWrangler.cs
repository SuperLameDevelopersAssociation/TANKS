using UnityEngine;
using System.Collections;
using TrueSync;

public class PlayerReadinessWrangler : TrueSyncBehaviour
{
    byte readinessCounter;
	PlayerManager playerManager;

    void Start()
    {
		playerManager = GameObject.Find ("GameManager").GetComponent<PlayerManager> ();
    }

    public override void OnSyncedStart()
    {
       // TrueSyncManager.PauseSimulation();
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
		//	TrueSyncManager.RunSimulation ();
			//playerManager.SpawnPlayers ();
		}
        else
            readinessCounter = 0;
    }

    IEnumerator UnPauseTimer()
    {
        yield return 5;
        TrueSyncManager.RunSimulation();
    }
}