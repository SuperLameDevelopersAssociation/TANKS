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

        if(!matchEnding)
		    matchTime.text = string.Format("{0:#00}:{1:00}", minutes, (int)seconds);
	}

	public IEnumerator MatchEnding()
	{
		if (!matchEnding) 
		{
			matchEnding = true;

			int player = pointsManager.PlayerThatWon ();
            if (player != 100)
                matchTime.text = "Player " + (player + 1) + " Won!";
            else
                matchTime.text = "Tie!";

            yield return new WaitForSeconds (matchEndingTime);
			SceneManager.LoadScene ("GameOver");
		}
	}
}
