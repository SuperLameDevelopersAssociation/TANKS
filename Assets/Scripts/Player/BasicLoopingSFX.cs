using UnityEngine;
using TrueSync;
using System.Collections;

public class BasicLoopingSFX : TrueSyncBehaviour {

    public AudioSource source;
    public AudioClip sfx;

    public override void OnSyncedStart()
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
