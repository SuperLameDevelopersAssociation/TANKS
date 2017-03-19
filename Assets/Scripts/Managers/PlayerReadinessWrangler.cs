using UnityEngine;
using System.Collections;
using TrueSync;

public class PlayerReadinessWrangler : TrueSyncBehaviour
{
    byte readinessCounter;

    void Start()
    {

    }

    public override void OnSyncedStart()
    {
        //TrueSyncManager.PauseSimulation();
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
            TrueSyncManager.RunSimulation();
        else
            readinessCounter = 0;
    }

    IEnumerator UnPauseTimer()
    {
        yield return 5;
        TrueSyncManager.RunSimulation();
    }
}