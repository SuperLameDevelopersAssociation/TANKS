using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Deathmatch : MonoBehaviour 
{
	public Text matchTime;
	public int matchTimeInMinutes;
	public byte killsToWin;
	public float matchEndingTime;

	[HideInInspector]
	public bool matchEnding;
	
	float minutes = 5;
	float seconds = 0;
	PointsManager pointsManager;

	void Start()
	{
		pointsManager = GameObject.Find ("GameManager").GetComponent<PointsManager> ();
		pointsManager.deathmatchActive = true;
		minutes = matchTimeInMinutes;
	}

	void Update()
	{

		if (seconds <= 0) 
		{
			minutes--;
			seconds = 59;
		} 
		else if ((int)seconds >= 0) 
		{
			seconds -= Time.deltaTime;
		}

		if (minutes <= 0 && seconds <= 0) 
		{
			StartCoroutine(MatchEnding ());
		}

		matchTime.text = string.Format("{0}:{1}", minutes, (int)seconds);
	}

	public IEnumerator MatchEnding()
	{
		if (!matchEnding) 
		{
			matchEnding = true;

			pointsManager.PlayerThatWon ();
			print ("Match Ending");
			yield return new WaitForSeconds (matchEndingTime);
			SceneManager.LoadScene ("GameOver");
		}
	}
}
