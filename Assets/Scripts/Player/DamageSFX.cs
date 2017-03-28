using UnityEngine;
using System.Collections;

public class DamageSFX : MonoBehaviour 
{

	public AudioSource mainSound;
	public AudioClip sizzle;
	public AudioClip bullet;
	public AudioClip burn;

	// Use this for initialization
	void Start () 
	{
        mainSound.loop = false;

        if (mainSound == null)
			Debug.LogError ("there is no main audio source attached to DamageSFX");
		else
			mainSound.playOnAwake = false;
	}
	
	public void PlayDamageSFX(string type)
	{
		if (type == "Projectile") {
			mainSound.clip = bullet;
			//mainSound.loop = false;
			mainSound.Play ();
		} else if (type == "Laser") {
			mainSound.clip = burn;
			//mainSound.loop = true;
			mainSound.Play ();
		} else if (type == "Flamethrower") {
			mainSound.clip = sizzle;
			//mainSound.loop = true;
			mainSound.Play ();
		} else
			Debug.LogError ("There are no sfx clips attatched");
	}

	public void StopDamageSFX()
	{
		if (mainSound.isPlaying)
			mainSound.Stop ();
	}
}
