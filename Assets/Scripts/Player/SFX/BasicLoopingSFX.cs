using UnityEngine;

public class BasicLoopingSFX : MonoBehaviour {

    public AudioSource source;
    public AudioClip sfx;

    void Start()
    {
        if (source == null)
        {
            Debug.LogError("Looping SFX has been added without a source.");
            source = gameObject.AddComponent<AudioSource>();
        }

        if (sfx == null)
        {
            Debug.LogError("Looping SFX has been added without a sfx.");
        }
        else
        {
            source.loop = true;
            source.clip = sfx;
            source.Play();
        }
    }
}