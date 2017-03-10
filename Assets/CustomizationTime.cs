using UnityEngine;
using System.Collections;
using  UnityEngine.UI;
using TrueSync;
using UnityEngine.SceneManagement;

public class CustomizationTime : TrueSyncBehaviour
{
	public int timeToCustomize;
	Text timer;

	public override void OnSyncedStart()
	{
		timer = GetComponent<Text> ();
		TrueSyncManager.SyncedStartCoroutine (TimeLeft ());
	}

	IEnumerator TimeLeft()
	{
		for (int i = timeToCustomize; i > 0; i--) 
		{
			timer.text = "Time Left: " + i;
			yield return 1;
		}
		SceneManager.LoadScene ("Game");
	}
}
