using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TrueSync;

public class Deathmatch : TrueSyncBehaviour
{
	public int matchTimeInMinutes;
	public byte killsToWin;
	public int matchEndingTime;

    [HideInInspector]
    public string matchTime;
    [HideInInspector]
    public bool matchEnding;
	
	FP minutes = 5;
	FP seconds = 0;
	PointsManager pointsManager;

    public string MatchTime {
        get { return matchTime; }
        set { value = matchTime; }
    }

    void Start()
    {
        pointsManager = GameObject.Find("GameManager").GetComponent<PointsManager>();
        pointsManager.deathmatchActive = true;
    }

	public override void OnSyncedStart()
	{
        minutes = matchTimeInMinutes;
    }

    public override void OnSyncedUpdate()
	{
		if (seconds <= 0) 
		{
			minutes = minutes - 1;
			seconds = 59;
		} 
		else if ((int)seconds >= 0) 
		{
			seconds -= TrueSyncManager.DeltaTime;
		}

		if (minutes <= 0 && seconds <= 0) 
		{
            TrueSyncManager.SyncedStartCoroutine(MatchEnding ());
		}

        if (!matchEnding)
        {
            matchTime = string.Format("{0:0#}{1:0#}", minutes, (int)seconds);
        }
            
	}

	public IEnumerator MatchEnding()
	{
		if (!matchEnding) 
		{
			matchEnding = true;

			int player = pointsManager.PlayerThatWon ();
            if (player != 100)
                matchTime = "Player " + (player + 1) + " Won!";
            else
                matchTime = "Tie!";

            yield return matchEndingTime;
			SceneManager.LoadScene ("GameOver");
		}
	}    
}