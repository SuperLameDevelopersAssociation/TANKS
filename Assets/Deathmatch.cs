using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Deathmatch : MonoBehaviour 
{
	public Text matchTime;
	public int matchTimeInMinutes;

	float minutes = 5;
	float seconds = 0;

	void Start()
	{
		minutes = matchTimeInMinutes;
	}

	void Update()
	{
		if (minutes <= 0) 
		{
			MatchEnding ();
		}

		if (seconds <= 0) 
		{
			minutes--;
			seconds = 59;
		} 
		else if ((int)seconds >= 0) 
		{
			seconds -= Time.deltaTime;
		}
		matchTime.text = string.Format("{0}:{1}", minutes, (int)seconds);
	}

	void MatchEnding()
	{
		print ("Match Ending");
		SceneManager.LoadScene ("GameOver");
	}
}
