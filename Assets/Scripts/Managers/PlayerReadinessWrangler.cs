using UnityEngine;
using System.Collections;
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

    void Start()
    {

    }

    public override void OnSyncedStart()
    {
        TrueSyncManager.PauseSimulation();
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
			StartCoroutine (Counter ());
		}
        else
            readinessCounter = 0;
    }

	IEnumerator Counter()
	{
		for (float i = timerLength; i > 0; i--) 
		{
			timeLeft.text = "" + i;
			yield return new WaitForSeconds(1);
		}

		readyToLoad = true;
		ready = true;

		TrueSyncManager.RunSimulation();
		startGameMode.SetActive (true);
	}
	public override void OnSyncedUpdate()
	{
		if (readyToLoad) {
			foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player")) {
				go.GetComponent<CustomizationManager> ().CustomizeTank ();
			}
		}
	}
}