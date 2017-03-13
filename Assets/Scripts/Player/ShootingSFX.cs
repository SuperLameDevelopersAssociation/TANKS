using UnityEngine;
using System.Collections;

public class ShootingSFX : MonoBehaviour {

    public AudioSource source;
    public AudioClip projectile;
    public AudioClip flamethrower;
    public AudioClip laser;

	// Use this for initialization
	void Start ()
    {
        if (source == null)
            Debug.LogError("There is no AudioSource attached to ShootingSFX");
        else
            source.playOnAwake = false;
	}

    public void PlayProjectileSFX()
    {
        source.clip = projectile;
        source.loop = false;
        source.Play();
    }

    public void PlaySustainedSFX(string sustainedType)
    {
        source.loop = true;

        if (sustainedType.Equals("Laser")) source.clip = laser;
        else if (sustainedType.Equals("Flamethrower")) source.clip = flamethrower;
        else Debug.LogError("Incorrect sustainedType variable");

        source.Play();
    }

    public void StopSustainedSFX()
    {
        if (source.isPlaying)
            source.Stop();
    }
}
